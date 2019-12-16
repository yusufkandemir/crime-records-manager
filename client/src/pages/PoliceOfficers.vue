<template>
  <q-page class="flex flex-center" padding>
    <q-table
      title="Police Officers"
      class="full-width q-my-lg"
      :grid="$q.screen.xs"
      row-key="Id"
      :data="data"
      :columns="columns"
      :pagination.sync="pagination"
      :rows-per-page-options="rowsPerPageOptions"
      :loading="loading"
      :filter="filter"
      @request="onRequest"
      binary-state-sort
      ref="dataTable"
    >
      <template v-slot:top-right>
        <q-input dense debounce="500" v-model="filter" placeholder="Search">
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>

        <q-btn
          class="q-ml-md"
          color="primary"
          icon="person_add"
          label="New Police Officer"
          @click="dialog = true"
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
    </q-table>
    <q-dialog v-model="dialog">
      <q-card class="q-pa-sm">
        <q-card-section>
          <span class="text-h5">{{ formTitle }}</span>
        </q-card-section>

        <q-card-section>
          <div class="row q-col-gutter-md">
            <div class="col-xs-12 col-sm-6 col-md-4">
              <q-input v-model="editedItem.Name" label="Name"></q-input>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
              <q-input v-model="editedItem.Surname" label="Surname"></q-input>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
              <q-input v-model="editedItem.Gender" label="Gender"></q-input>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
              <q-input
                v-model="editedItem.BirthDate"
                label="Birth Date"
                mask="date"
                :rules="['date']"
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      ref="birthDateProxy"
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date
                        v-model="editedItem.BirthDate"
                        @input="() => $refs.birthDateProxy.hide()"
                      />
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
              <q-input v-model="editedItem.Rank" label="Rank"></q-input>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
              <c-entity-selector
                v-model="editedItem.StationId"
                label="Station"
                entity="PoliceStation"
                value-column="Id"
                display-column="Name"
                ref="stationSelector"
              >
                <template v-slot:prepend>
                  <q-icon name="emoji_transportation" @click.stop />
                </template>
              </c-entity-selector>
            </div>
          </div>
        </q-card-section>

        <q-card-actions>
          <q-space />
          <q-btn flat color="primary" :loading="loading" @click="close">Cancel</q-btn>
          <q-btn flat color="primary" :loading="loading" @click="save">Save</q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { date } from 'quasar'

const { formatDate } = date

import { CEntitySelector } from '../components/CEntitySelector'

export default {
  name: 'PoliceOfficersPage',
  components: {
    CEntitySelector
  },
  data () {
    return {
      data: [],
      editedIndex: -1,
      editedItem: {
        Name: '',
        Surname: '',
        Gender: 'Man',
        BirthDate: new Date(),
        Rank: 'Officer',
        StationId: 0
      },
      defaultItem: {
        Name: '',
        Surname: '',
        Gender: 'Man',
        BirthDate: new Date(),
        Rank: 'Officer',
        StationId: 0
      },
      filter: '',
      pagination: {
        page: 1,
        sortBy: 'name',
        descending: false,
        rowsPerPage: this.$q.screen.xs ? 12 : 24,
        rowsNumber: 0
      },
      rowsPerPageOptions: [12, 24, 36, 48],
      columns: [
        {
          name: 'name',
          label: 'Name',
          field: 'Name',
          sortable: true,
          filterable: true
        },
        {
          name: 'surname',
          label: 'Surname',
          field: 'Surname',
          sortable: true,
          filterable: true
        },
        {
          name: 'gender',
          label: 'Gender',
          field: 'Gender',
          sortable: true,
          filterable: true
        },
        {
          name: 'birthDate',
          label: 'Birth Date',
          field: 'BirthDate',
          format: value => value ? formatDate(value, 'YYYY/MM/DD') : null,
          sortable: true
        },
        {
          name: 'rank',
          label: 'Rank',
          field: 'Rank',
          sortable: true,
          filterable: true
        },
        {
          name: 'actions',
          label: 'Actions',
          sortable: false
        }
      ],
      loading: false,
      dialog: false
    }
  },
  computed: {
    formTitle () {
      return this.editedIndex === -1 ? 'New Police Officer' : 'Edit Police Officer'
    }
  },
  watch: {
    async dialog (val) {
      val || this.close()

      // If updating an item, and it has a StationId set
      if (this.editedIndex > -1 && this.editedItem.StationId > 0) {
        // Fetch and update the options to prevent seeing just the id
        // FIXME: the needed result might not be in the response because of pagination limits
        await this.$nextTick()
        await this.$refs.stationSelector.refreshOptions()
      }
    }
  },
  mounted () {
    this.onRequest({
      pagination: this.pagination,
      filter: ''
    })
  },
  methods: {
    async onRequest ({ pagination, filter }) {
      let { page, rowsPerPage, rowsNumber, sortBy, descending } = pagination

      this.loading = true

      // get all rows if "All" (0) is selected
      let fetchCount = rowsPerPage === 0 ? rowsNumber : rowsPerPage

      // calculate starting row of data
      let startRow = (page - 1) * rowsPerPage

      let returnedData = await this.fetchFromServer(startRow, fetchCount, filter, sortBy, descending)

      // update rowsCount with appropriate value
      this.pagination.rowsNumber = parseInt(returnedData['@odata.count'])

      // clear out existing data and add new
      this.data.splice(0, this.data.length, ...returnedData.value)

      // don't forget to update local pagination object
      this.pagination.page = page
      this.pagination.rowsPerPage = rowsPerPage
      this.pagination.sortBy = sortBy
      this.pagination.descending = descending

      // ...and turn of loading indicator
      this.loading = false
    },

    async fetchFromServer (startRow, count, filter, sortBy, descending) {
      const params = new URLSearchParams({
        $skip: startRow,
        $top: count,
        $count: true
      })

      if (sortBy) {
        params.append('$orderBy', `${sortBy} ${descending ? 'desc' : 'asc'}`)
      }

      if (filter) {
        const filterQuery = this.columns
          .filter(column => column.filterable)
          .map(column => `contains(${column.field}, '${filter}')`)
          .join(' or ')

        params.append('$filter', filterQuery)
      }

      let url = `/api/PoliceOfficers?${params.toString()}`

      const response = await fetch(url)
      const data = await response.json()

      return data
    },

    editItem (item) {
      this.editedIndex = this.data.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },

    async deleteItem (item) {
      if (!confirm('Are you sure you want to delete this item?')) return

      await fetch(`/api/PoliceOfficers/${item.Id}`, {
        method: 'DELETE'
      })

      // Trigger table for update
      this.$refs.dataTable.requestServerInteraction({
        pagination: this.pagination,
        filter: this.filter
      })
    },

    close () {
      this.dialog = false
      this.editedItem = Object.assign({}, this.defaultItem)
      this.editedIndex = -1
    },

    async save () {
      const isUpdating = this.editedIndex > -1

      this.loading = true
      await this.pushToServer(this.editedItem, isUpdating)

      // Trigger table for update
      this.$refs.dataTable.requestServerInteraction({
        pagination: this.pagination,
        filter: this.filter
      })

      this.loading = false

      this.close()
    },

    async pushToServer (item, update = false) {
      let url = `/api/PoliceOfficers/${update ? item.Id : ''}`

      return fetch(url, {
        method: update ? 'PUT' : 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          ...item,
          BirthDate: item.BirthDate !== null ? (new Date(item.BirthDate)).toISOString() : null,
          StationId: item.StationId !== null ? item.StationId.value : null
        })
      })
    }
  }
}
</script>

<style lang="sass">
  // Center bottom content
  .q-table__container .q-table__bottom
    justify-content: center
</style>
