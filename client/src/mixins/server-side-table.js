export default {
  data () {
    return {
      entityName: '',
      data: [],
      columns: [],
      pagination: {},
      loading: false
    }
  },
  methods: {
    async onRequest ({ pagination, filter }) {
      let { page, rowsPerPage, rowsNumber, sortBy, descending } = pagination

      this.loading = true

      // get all rows if "All" (0) is selected
      let fetchCount = rowsPerPage === 0 ? rowsNumber : rowsPerPage

      // calculate starting row of data
      let startRow = (page - 1) * rowsPerPage

      let returnedData = await this.fetchDataFromServer(startRow, fetchCount, filter, sortBy, descending)

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
        const filterQuery = this.columns
          .filter(column => column.filterable)
          .map(column => `contains(${column.field}, '${filter}')`)
          .join(' or ')

        params.append('$filter', filterQuery)
      }

      let url = `/api/${this.entityName}s?${params.toString()}`

      const response = await fetch(url)
      const data = await response.json()

      return data
    }
  }
}
