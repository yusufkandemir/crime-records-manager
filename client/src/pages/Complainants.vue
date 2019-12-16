<template>
  <q-page class="flex flex-center" padding>
    <c-serverside-crud-table
      :entity="{
        dataName: 'Complainant',
        displayName: 'Complainant'
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
          <q-input
            v-model="editedItem.PhoneNumber"
            label="Phone Number"
            mask="(0###) ### ## ##"
            :rules="[
              value => value.match(/\(0\d{3}\) \d{3} \d{2} \d{2}/) || 'You need to enter a valid phone number'
            ]"
          ></q-input>
        </div>
      </div>
    </c-serverside-crud-table>
  </q-page>
</template>

<script>
import { date } from 'quasar'
const { formatDate } = date

import { CDateInput } from '../components/CDateInput'
import { CServersideCrudTable } from '../components/CServersideCrudTable'

export default {
  name: 'PoliceOfficersPage',
  components: {
    CDateInput,
    CServersideCrudTable
  },
  data () {
    return {
      items: [],
      editedIndex: -1,
      editedItem: {
        Name: '',
        Surname: '',
        Gender: 'Man',
        BirthDate: new Date(),
        PhoneNumber: ''
      },
      defaultItem: {
        Name: '',
        Surname: '',
        Gender: 'Man',
        BirthDate: new Date(),
        PhoneNumber: ''
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
          name: 'phoneNumber',
          label: 'Phone Number',
          field: 'PhoneNumber',
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
  methods: {
    processData (item) {
      const result = { ...item }

      if (result.BirthDate !== null || result.BirthDate !== undefined) {
        result.BirthDate = (new Date(result.BirthDate)).toISOString()
      }

      return result
    }
  }
}
</script>
