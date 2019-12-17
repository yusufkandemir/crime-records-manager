<template>
  <q-input
    mask="date"
    :rules="['date']"
    v-model="localValue"
    v-bind="$attrs"
    v-on="$listeners"
  >
    <template v-slot:append>
      <q-icon name="event" class="cursor-pointer">
        <q-popup-proxy ref="birthDateProxy" transition-show="scale" transition-hide="scale">
          <q-date v-model="localValue" @input="onDateInput" />
        </q-popup-proxy>
      </q-icon>
    </template>

    <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
      <slot :name="slot" v-bind="scope" />
    </template>
  </q-input>
</template>

<script>
export default {
  name: 'CDateInput',
  props: ['value'],
  computed: {
    localValue: {
      get () {
        return this.value
      },
      set (value) {
        this.onInput(value)
      }
    }
  },
  methods: {
    onInput (value) {
      this.$emit('input', value)
    },
    onDateInput () {
      this.$refs.birthDateProxy.hide()
    }
  }
}
</script>
