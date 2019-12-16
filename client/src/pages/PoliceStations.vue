<template>
  <q-page class="flex flex-center" padding>
    <c-serverside-crud-table
      :entity="{
        dataName: 'PoliceStation',
        displayName: 'Police Station'
      }"
      :edited-index.sync="editedIndex"
      :edited-item.sync="editedItem"
      :default-item.sync="defaultItem"
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
        <div class="col-xs-12">
          <q-input v-model="editedItem.Name" label="Name"></q-input>
        </div>
        <div class="col-xs-12">
          <q-input v-model="editedItem.Address" label="Address" autogrow>
            <template v-slot:prepend>
              <q-icon name="room" />
            </template>
          </q-input>
        </div>
      </div>
    </c-serverside-crud-table>
  </q-page>
</template>

<script>
import { CServersideCrudTable } from '../components/CServersideCrudTable'

export default {
  name: 'PoliceStationsPage',
  components: {
    CServersideCrudTable
  },
  data () {
    return {
      items: [],
      editedIndex: -1,
      editedItem: {
        Name: '',
        Address: ''
      },
      defaultItem: {
        Name: '',
        Address: ''
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
          name: 'address',
          label: 'Address',
          field: 'Address',
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
  }
}
</script>

<style lang="sass">
  // Center bottom content
  .q-table__container .q-table__bottom
    justify-content: center
</style>
