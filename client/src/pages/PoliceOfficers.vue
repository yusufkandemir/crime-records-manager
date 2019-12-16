<template>
  <q-page class="flex flex-center" padding>
    <c-serverside-crud-table
      :entity="{
        dataName: 'PoliceOfficer',
        displayName: 'Police Officer'
      }"
      :edited-index.sync="editedIndex"
      :edited-item.sync="editedItem"
      :default-item.sync="defaultItem"
      :process-data="processData"
      class="full-width q-my-lg"
      :grid="$q.screen.xs"
      row-key="Id"
      :items="items"
      :columns-config="columns"
      :pagination-config.sync="pagination"
      :rows-per-page-options="rowsPerPageOptions"
      :is-loading.sync="loading"
      :is-dialog-open.sync="dialog"
      :filter.sync="filter"
    >
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
          <c-date-input v-model="editedItem.BirthDate" label="Birth Date"></c-date-input>
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
    </c-serverside-crud-table>
  </q-page>
</template>

<script>
import { date } from 'quasar'
const { formatDate } = date

import { CDateInput } from '../components/CDateInput'
import { CEntitySelector } from '../components/CEntitySelector'
import { CServersideCrudTable } from '../components/CServersideCrudTable'

export default {
  name: 'PoliceOfficersPage',
  components: {
    CEntitySelector,
    CDateInput,
    CServersideCrudTable
  },
  data () {
    return {
      entityName: 'PoliceOfficer',
      items: [],
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
  watch: {
    async dialog (val) {
      // If updating an item, and it has a StationId set
      if (val && this.editedIndex > -1 && this.editedItem.StationId > 0) {
        // Fetch and update the options to prevent seeing just the id
        // FIXME: the needed result might not be in the response because of pagination limits
        await this.$nextTick()
        await this.$refs.stationSelector.refreshOptions()
      }
    }
  },
  methods: {
    processData (item) {
      const result = { ...item }
      if (result.BirthDate !== null || result.BirthDate !== undefined) {
        result.BirthDate = (new Date(result.BirthDate)).toISOString()
      }

      if (result.StationId !== null || result.StationId !== undefined) {
        result.StationId = result.StationId.value
      }

      return result
    }
  }
}
</script>

<style lang="sass">
  // Center bottom content
  .q-table__container .q-table__bottom
    justify-content: center
</style>
