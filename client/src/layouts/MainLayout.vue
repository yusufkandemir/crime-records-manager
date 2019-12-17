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

        <q-avatar class="q-ml-sm">
          <img src="statics/logo.png" />
        </q-avatar>
        <q-toolbar-title class="q-pl-sm">Crime Records Manager</q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="isLeftDrawerOpen" elevated show-if-above>
      <q-list>
        <q-item to="/" exact>
          <q-item-section avatar>
            <q-icon name="dashboard" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Dashboard</q-item-label>
          </q-item-section>
        </q-item>
        <q-item to="/complainants">
          <q-item-section avatar>
            <q-icon name="record_voice_over" />
          </q-item-section>
          <q-item-section>
            <q-item-label>Complainants</q-item-label>
          </q-item-section>
        </q-item>
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
