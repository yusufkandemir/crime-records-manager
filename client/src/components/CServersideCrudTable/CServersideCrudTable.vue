<template>
  <div>
    <q-table
      :title="entity.displayName + 's'"
      row-key="Id"
      :data="localData"
      :columns="localColumns"
      :pagination.sync="localPagination"
      :filter="localFilter"
      :loading="localLoading"
      @request="onRequest"
      binary-state-sort
      ref="dataTable"
      v-bind="$attrs"
      v-on="$listeners"
    >
      <template v-slot:top-right>
        <q-input dense debounce="500" v-model="localFilter" placeholder="Search">
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>

        <q-btn
          class="q-ml-md"
          color="primary"
          icon="add"
          :label="'New ' + entity.displayName"
          @click="localDialog = true"
        />
      </template>

      <template v-slot:body="props">
        <q-tr :key="props.key" :props="props">
          <q-td v-for="(column, name) in props.colsMap" :key="name" :props="props">
            <template v-if="column.name === 'actions'">
              <q-btn
                round
                flat
                size="sm"
                class="q-mx-none"
                icon="edit"
                @click="editItem(props.row)"
              />
              <q-btn
                round
                flat
                size="sm"
                class="q-mx-none"
                icon="delete"
                @click="deleteItem(props.row)"
              />
            </template>
            <template v-else>{{ (column.format || (x => x))(props.row[column.field]) }}</template>
          </q-td>
        </q-tr>
      </template>

      <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
        <slot :name="slot" v-bind="scope" />
      </template>
    </q-table>
    <q-dialog v-model="localDialog">
      <q-card class="q-pa-sm">
        <q-card-section>
          <span class="text-h5">{{ formTitle }}</span>
        </q-card-section>

        <q-card-section>
          <slot></slot>
        </q-card-section>

        <q-card-actions>
          <q-space />
          <q-btn flat color="primary" :loading="localLoading" @click="close">Cancel</q-btn>
          <q-btn flat color="primary" :loading="localLoading" @click="save">Save</q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
export default {
  name: 'PoliceOfficersPage',
  props: {
    entity: {
      type: Object,
      required: true,
      validator (entity) {
        return (
          typeof entity.dataName === 'string' &&
          typeof entity.displayName === 'string'
        )
      }
    },
    editedIndex: {
      type: Number,
      required: true
    },
    editedItem: {
      type: Object,
      required: true
    },
    defaultItem: {
      type: Object,
      required: true
    },
    filter: {
      type: String,
      default: ''
    },
    items: {
      type: Array,
      required: true
    },
    paginationConfig: {
      type: Object,
      required: true
    },
    columnsConfig: {
      type: Array,
      required: true
    },
    isLoading: {
      type: Boolean,
      default: false
    },
    isDialogOpen: {
      type: Boolean,
      default: false
    },
    processData: {
      type: Function,
      default: data => data
    }
  },
  computed: {
    formTitle () {
      return (this.editedIndex === -1 ? 'New ' : 'Edit ') + this.entity.displayName
    },
    localLoading: {
      get () {
        return this.isLoading
      },
      set (value) {
        this.$emit('update:isLoading', value)
      }
    },
    localPagination: {
      get () {
        return this.paginationConfig
      },
      set (value) {
        this.$emit('update:paginationConfig', value)
      }
    },
    localColumns: {
      get () {
        return this.columnsConfig
      },
      set (value) {
        this.$emit('update:columnsConfig', value)
      }
    },
    localData: {
      get () {
        return this.items
      },
      set (value) {
        this.$emit('update:items', value)
      }
    },
    localDialog: {
      get () {
        return this.isDialogOpen
      },
      set (value) {
        this.$emit('update:isDialogOpen', value)
      }
    },
    localFilter: {
      get () {
        return this.filter
      },
      set (value) {
        this.$emit('update:filter', value)
      }
    }
  },
  watch: {
    async localDialog (val) {
      val || this.close()
    }
  },
  mounted () {
    this.onRequest({
      pagination: this.localPagination,
      filter: ''
    })
  },
  methods: {
    editItem (item) {
      this.$emit('update:editedIndex', this.localData.indexOf(item))
      this.$emit('update:editedItem', Object.assign({}, item))
      this.localDialog = true
    },

    async deleteItem (item) {
      if (!confirm('Are you sure you want to delete this item?')) return

      await fetch(`/api/${this.entity.dataName}s/${item.Id}`, {
        method: 'DELETE'
      })

      // Trigger table for update
      this.$refs.dataTable.requestServerInteraction({
        pagination: this.localPagination,
        filter: this.localFilter
      })
    },

    close () {
      this.localDialog = false
      this.$emit('update:editedIndex', -1)
      this.$emit('update:editedItem', Object.assign({}, this.defaultItem))
    },

    async save () {
      const isUpdating = this.editedIndex > -1

      this.localLoading = true
      await this.pushToServer(this.editedItem, isUpdating)

      // Trigger table for update
      this.$refs.dataTable.requestServerInteraction({
        pagination: this.localPagination,
        filter: this.localFilter
      })

      this.localLoading = false

      this.close()
    },

    async pushToServer (item, update = false) {
      let url = `/api/${this.entity.dataName}s/${update ? item.Id : ''}`

      return fetch(url, {
        method: update ? 'PUT' : 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.processData(item))
      })
    },
    // mixin:
    async onRequest ({ pagination, filter }) {
      let { page, rowsPerPage, rowsNumber, sortBy, descending } = pagination

      this.localLoading = true

      // get all rows if "All" (0) is selected
      let fetchCount = rowsPerPage === 0 ? rowsNumber : rowsPerPage

      // calculate starting row of data
      let startRow = (page - 1) * rowsPerPage

      let returnedData = await this.fetchDataFromServer(startRow, fetchCount, filter, sortBy, descending)

      // update rowsCount with appropriate value
      this.localPagination.rowsNumber = parseInt(returnedData['@odata.count'])

      // clear out existing data and add new
      this.items.splice(0, this.items.length, ...returnedData.value)

      // don't forget to update local pagination object
      this.localPagination.page = page
      this.localPagination.rowsPerPage = rowsPerPage
      this.localPagination.sortBy = sortBy
      this.localPagination.descending = descending

      // ...and turn of loading indicator
      this.localLoading = false
    },

    async fetchDataFromServer (startRow, count, filter, sortBy, descending) {
      const params = new URLSearchParams({
        $skip: startRow,
        $top: count,
        $count: true
      })

      if (sortBy) {
        params.append('$orderBy', `${sortBy} ${descending ? 'desc' : 'asc'}`)
      }

      if (filter) {
        const filterQuery = this.localColumns
          .filter(column => column.filterable)
          .map(column => `contains(${column.field}, '${filter}')`)
          .join(' or ')

        params.append('$filter', filterQuery)
      }

      let url = `/api/${this.entity.dataName}s?${params.toString()}`

      const response = await fetch(url)
      const data = await response.json()

      return data
    }
  }
}
</script>

<style lang="sass">
  // Center bottom content
  .q-table__container .q-table__bottom
    justify-content: center
</style>
