<template>
  <q-select
    use-input
    fill-input
    hide-selected
    input-debounce="500"
    :options="options"
    @filter="filterOptions"
    map-options
    :value="value"
    @input="onInput"
    v-bind="$attrs"
    v-on="$listeners"
  >
    <template v-slot:no-option>
      <q-item>
        <q-item-section class="text-grey">No results</q-item-section>
      </q-item>
    </template>

    <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
      <slot :name="slot" v-bind="scope" />
    </template>
  </q-select>
</template>

<script>
export default {
  name: 'CEntitySelector',
  props: {
    value: {
      required: true
    },
    entity: {
      type: String,
      required: true
    },
    valueColumn: {
      type: String,
      default: 'Id'
    },
    displayColumn: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      options: []
    }
  },
  methods: {
    onInput (value) {
      this.$emit('input', value)
    },

    async refreshOptions (filter) {
      const params = new URLSearchParams()

      if (filter) {
        params.append('$filter', `contains(${this.displayColumn}, '${filter}')`)
      }

      let url = `/api/${this.entity}s?${params.toString()}`

      const response = await fetch(url)
      const { value: entities } = await response.json()

      this.options = entities.map(entity => ({
        value: entity[this.valueColumn],
        label: entity[this.displayColumn]
      }))

      return this.options
    },

    filterOptions (filter, update, abort) {
      update(async () => {
        this.refreshOptions(filter)
      })
    }
  }
}
</script>
