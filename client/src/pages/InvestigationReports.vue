<template>
  <q-page class="flex flex-center" padding>
    <c-serverside-crud-table
      :entity="{
        dataName: 'InvestigationReport',
        displayName: 'Investigation Report'
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
          <q-input v-model="editedItem.Act" label="Act"></q-input>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
          <q-input v-model="editedItem.Address" label="Address"></q-input>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
          <c-date-input v-model="editedItem.OccuredAt" label="Occured Time"></c-date-input>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
          <c-date-input v-model="editedItem.WrittenAt " label="Written Time"></c-date-input>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-4">
          <c-entity-selector
            v-model="editedItem.ReporterId"
            label="Officer"
            entity="PoliceOfficer"
            value-column="Id"
            display-column="Name"
            ref="officerSelector"
          >
            <template v-slot:prepend>
              <q-icon name="person" @click.stop />
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
  name: 'InvestigationReportsPage',
  components: {
    CEntitySelector,
    CServersideCrudTable
  },
  data () {
    return {
      items: [],
      editedIndex: -1,
      editedItem: {
        Act: '',
        Address: '',
        OccuredAt: new Date(),
        WrittenAt: new Date(),
        ReporterId: 0
      },
      defaultItem: {
        Act: '',
        Address: '',
        OccuredAt: new Date(),
        BirthDate: new Date(),
        ReporterId: 0
      },
      filter: '',
      pagination: {
        page: 1,
        sortBy: 'act',
        descending: false,
        rowsPerPage: this.$q.screen.xs ? 12 : 24,
        rowsNumber: 0
      },
      rowsPerPageOptions: [12, 24, 36, 48],
      columns: [
        {
          name: 'act',
          label: 'Act',
          field: 'Act',
          sortable: true,
          filterable: true
        },
        {
          name: 'address',
          label: 'Address',
          field: 'Address ',
          sortable: true,
          filterable: true
        },
        {
          name: 'occuredAt',
          label: 'Occured At',
          field: 'OccuredAt',
          format: value => value ? formatDate(value, 'YYYY/MM/DD') : null,
          sortable: true
        },
        {
          name: 'writtenAt',
          label: 'Written At',
          field: 'WrittenAt',
          format: value => value ? formatDate(value, 'YYYY/MM/DD') : null,
          sortable: true
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
      // If updating an item, and it has a ReporterId  set
      if (val && this.editedIndex > -1 && this.editedItem.ReporterId > 0) {
        // Fetch and update the options to prevent seeing just the id
        // FIXME: the needed result might not be in the response because of pagination limits
        await this.$nextTick()
        await this.$refs.officerSelector.refreshOptions()
      }
    }
  },
  methods: {
    processData (item) {
      const result = { ...item }

      if (result.OccuredAt !== null || result.OccuredAt !== undefined) {
        result.OccuredAt = (new Date(result.OccuredAt)).toISOString()
      }

      if (result.WrittenAt !== null || result.WrittenAt !== undefined) {
        result.WrittenAt = (new Date(result.WrittenAt)).toISOString()
      }

      if (result.ReporterId !== null || result.ReporterId !== undefined) {
        result.ReporterId = result.ReporterId.value
      }

      return result
    }
  }
}
</script>
