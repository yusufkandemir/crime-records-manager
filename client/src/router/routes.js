
const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/Index.vue') },
      { path: 'police-officers', component: () => import('pages/PoliceOfficers.vue') },
      { path: 'police-stations', component: () => import('pages/PoliceStations.vue') },
      { path: 'investigation-reports', component: () => import('pages/InvestigationReports.vue') },
      { path: 'suspects', component: () => import('pages/Suspects.vue') },
      { path: 'complainants', component: () => import('pages/Complainants.vue') },
      { path: 'complaints', component: () => import('pages/Complaints.vue') }
    ]
  }
]

// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}

export default routes
