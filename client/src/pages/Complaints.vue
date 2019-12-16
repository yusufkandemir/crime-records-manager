<template>
  <q-page class="flex flex-center" padding>
    <c-serverside-crud-table
      :entity="{
        dataName: 'Complaint',
        displayName: 'Complaint'
      }"
      :edited-index.sync="editedIndex"
      :edited-item.sync="editedItem"
      :default-item.sync="defaultItem"
      :process-data="processData"
      class="full-width q-my-lg"
      :grid="$q.screen.xs"
      :items="items"
      :columns-config="columns"
      :pagination-config.sync="pagination"
      :rows-per-page-options="rowsPerPageOptions"
      :is-loading.sync="loading"
      :is-dialog-open.sync="dialog"
      :filter.sync="filter"
    >
      <div class="row q-col-gutter-md">
        <div class="col-xs-12 col-sm-6">
          <q-input v-model="editedItem.Place" label="Place">
            <template v-slot:prepend>
              <q-icon name="room" @click.stop />
            </template>
          </q-input>
        </div>
        <div class="col-xs-12 col-sm-6">
          <c-date-input v-model="editedItem.FiledAt" label="Filed At"></c-date-input>
        </div>
        <div class="col-xs-12">
          <q-input type="textarea" v-model="editedItem.Details" label="Details"></q-input>
        </div>
        <div class="col-xs-12 col-sm-6">
          <c-entity-selector
            v-model="editedItem.ComplainantId"
            label="Complainant"
            entity="Complainant"
            value-column="Id"
            display-column="Name"
            ref="complainantSelector"
          >
            <template v-slot:prepend>
              <q-icon name="record_voice_over" @click.stop />
            </template>
          </c-entity-selector>
        </div>
        <div class="col-xs-12 col-sm-6">
          <c-entity-selector
            v-model="editedItem.PoliceStationId"
            label="Police Station"
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
    CDateInput,
    CEntitySelector,
    CServersideCrudTable
  },
  data () {
    return {
      items: [],
      editedIndex: -1,
      editedItem: {
        Place: '',
        FiledAt: new Date(),
        Details: '',
        ComplainantId: 0,
        PoliceStationId: 0
      },
      defaultItem: {
        Place: '',
        FiledAt: new Date(),
        Details: '',
        ComplainantId: 0,
        PoliceStationId: 0
      },
      filter: '',
      pagination: {
        page: 1,
        sortBy: 'filedAt',
        descending: false,
        rowsPerPage: this.$q.screen.xs ? 12 : 24,
        rowsNumber: 0
      },
      rowsPerPageOptions: [12, 24, 36, 48],
      columns: [
        {
          name: 'place',
          label: 'Place',
          field: 'Place',
          sortable: true,
          filterable: true
        },
        {
          name: 'filedAt',
          label: 'Filed At',
          field: 'FiledAt',
          format: value => value ? formatDate(value, 'YYYY/MM/DD') : null,
          sortable: true
        },
        {
          name: 'details',
          label: 'Details',
          field: 'Details',
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
      if (val && this.editedIndex > -1) {
        // Fetch and update the options to prevent seeing just the id
        // FIXME: the needed result might not be in the response because of pagination limits
        await this.$nextTick()
        this.$refs.complainantSelector.refreshOptions()
        this.$refs.stationSelector.refreshOptions()
      }
    }
  },
  methods: {
    processData (item) {
      const result = { ...item }

      if (result.FiledAt !== null || result.FiledAt !== undefined) {
        result.FiledAt = (new Date(result.FiledAt)).toISOString()
      }

      if (result.ComplainantId !== null || result.ComplainantId !== undefined) {
        result.ComplainantId = result.ComplainantId.value
      }

      if (result.PoliceStationId !== null || result.PoliceStationId !== undefined) {
        result.PoliceStationId = result.PoliceStationId.value
      }

      return result
    }
  }
}
</script>
