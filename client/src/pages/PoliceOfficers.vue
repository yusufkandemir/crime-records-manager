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
    >
      <template v-slot:top-right>
        <q-input dense debounce="500" v-model="filter" placeholder="Search">
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>
      </template>
    </q-table>
  </q-page>
</template>

<script>
import { date } from 'quasar'

const { formatDate } = date

export default {
  name: 'PoliceOfficersPage',
  data () {
    return {
      data: [],
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
        }
      ],
      loading: false
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
    }
  }
}
</script>

<style lang="sass">
  // Center bottom content
  .q-table__container .q-table__bottom
    justify-content: center
</style>
