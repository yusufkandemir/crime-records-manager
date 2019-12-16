<template>
  <q-layout view="hHh LpR lFr">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="isLeftDrawerOpen = !isLeftDrawerOpen"
        />

        <q-toolbar-title>
          Crime Records Manager
        </q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="isLeftDrawerOpen"
      overlay
      elevated
      show-if-above
    >
      <q-list>
        <q-item to="/complaints">
          <q-item-section avatar>
            <q-icon name="assignment_late" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Complaints</q-item-label>
          </q-item-section>
        </q-item>
        <q-item to="/suspects">
          <q-item-section avatar>
            <q-icon name="directions_run" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Suspects</q-item-label>
          </q-item-section>
        </q-item>
        <q-item to="/investigation-reports">
          <q-item-section avatar>
            <q-icon name="assignment" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Investigation Reports</q-item-label>
          </q-item-section>
        </q-item>
        <q-item to="/police-officers">
          <q-item-section avatar>
            <q-icon name="people" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Police Officers</q-item-label>
          </q-item-section>
        </q-item>
        <q-item to="/police-stations">
          <q-item-section avatar>
            <q-icon name="emoji_transportation" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Police Stations</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
export default {
  name: 'MainLayout',

  data () {
    return {
      isLeftDrawerOpen: false
    }
  }
}
</script>
