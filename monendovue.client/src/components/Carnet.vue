<template>
  <section class="flex flex-wrap h-auto container mx-auto mt-16 py-8 bg-clearer rounded-3xl shadow-md ml-auto">
    <div class="w-full mb-8">
      <div class="flex gap-8 responsive-layout">
        <h1 class="text-3xl md:mb-4">Aujourd'hui - {{ formatDate(value || new Date()) }}</h1>
        <!--        <Popover>-->
        <!--          <div class="relative">-->
        <!--            <PopoverTrigger as-child>-->
        <!--              <Button-->
        <!--                  variant="custom"-->
        <!--                  :class="cn(-->
        <!--    'w-[280px] justify-start text-left font-normal',-->
        <!--  )"-->
        <!--              >-->
        <!--                <CalendarIcon class="mr-2 h-4 w-4"/>-->
        <!--                {{ value ? df.format(value.toDate(getLocalTimeZone())) : "Sélectionnez une date" }}-->
        <!--              </Button>-->
        <!--            </PopoverTrigger>-->
        <!--            <PopoverContent class="w-auto p-0 absolute left-0">-->
        <!--              <Calendar v-model="value" initial-focus locale="fr"/>-->
        <!--            </PopoverContent>-->
        <!--          </div>-->
        <!--        </Popover>-->
      </div>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 responsive-layout">
        <router-link to="/douleurs">
          <Card>
            <CardHeader>
              <i class="material-symbols-outlined" style="font-size: 48px;">sick</i>
              <CardTitle>Douleurs</CardTitle>
            </CardHeader>
            <div v-if="isLoading" class="px-4">Chargement des données...</div>
            <CardContent v-else>
              <p v-if="lastDouleurEntry">
                Dernière douleur ({{ lastDouleurEntry.typeDouleur }}) à <span class="highlight">{{
                  lastDouleurEntry.time
                }}</span> le {{ lastDouleurEntry.date }}
              </p>
              <p v-else>Pas de données</p>
            </CardContent>
          </Card>
        </router-link>
        <router-link to="/activite">
          <Card>
            <CardHeader>
              <i class="material-symbols-outlined" style="font-size: 48px;">directions_run</i>
              <CardTitle>Activité Physique</CardTitle>
            </CardHeader>
            <div v-if="isLoading" class="px-4">Chargement des données...</div>
            <CardContent v-else>
              <p v-if="lastActiviteEntry">
                Dernière activité ({{ lastActiviteEntry.typeActivite }}) à <span
                  class="highlight">{{ lastActiviteEntry.time }}</span> le {{ lastActiviteEntry.date }}
              </p>
              <p v-else>Pas de données</p>
            </CardContent>
          </Card>
        </router-link>
        <router-link to="/medicaments">
          <Card>
            <CardHeader>
              <i class="material-symbols-outlined" style="font-size: 48px;">medical_services</i>
              <CardTitle>Traitements</CardTitle>
            </CardHeader>
            <CardContent>
              <p v-if="lastMedicamentEntry">Dernière prise ({{ lastMedicamentEntry.nom }}) à <span
                  class="highlight">{{ lastMedicamentEntry.heure }}</span> le {{ lastMedicamentEntry.date }}
              </p>
              <p v-else>Pas de données</p>
            </CardContent>
          </Card>
        </router-link>
        <router-link to="/transit">
          <Card>
            <CardHeader>
              <i class="material-symbols-outlined" style="font-size: 48px;">gastroenterology</i>
              <CardTitle>Transit</CardTitle>
            </CardHeader>
<!--            <CardContent>-->
<!--              <p v-if="lastMedicamentEntry">Dernière prise ({{ lastMedicamentEntry.nom }}) à <span-->
<!--                  class="highlight">{{ lastMedicamentEntry.heure }}</span> le {{ lastMedicamentEntry.date }}-->
<!--              </p>-->
<!--              <p v-else>Pas de données</p>-->
<!--            </CardContent>-->
          </Card>
        </router-link>
        <!--        <Card>-->
        <!--          <CardHeader>-->
        <!--            <i class="material-symbols-outlined" style="font-size: 48px;">menstrual_health</i>-->
        <!--            <CardTitle>Cycle menstruel</CardTitle>-->
        <!--            <i class="material-symbols-outlined ml-auto">analytics</i>-->
        <!--          </CardHeader>-->
        <!--          <CardContent>-->
        <!--            Règles dans <span class="highlight">3 jours</span>-->
        <!--          </CardContent>-->
        <!--        </Card>-->
        <router-link to="/agenda" class="w-full col-span-3">
          <Card>
            <CardHeader class="w-full">
              <i class="material-symbols-outlined" style="font-size: 48px;">event</i>
              <CardTitle>Mes prochains RDV</CardTitle>
            </CardHeader>
            <CardContent class="flex flex-col md:flex-row text-center gap-4 md:gap-16">
              <div v-if="isLoading" class="px-4">Chargement des données...</div>
              <div v-else-if="upcomingEvents.length === 0" class="text-start">Pas de rendez-vous à venir</div>
              <div v-else v-for="event in upcomingEvents" :key="event.id"
                   class="rounded-lg shadow-md p-4 mb-4 bg-white flex flex-col min-w-40  justify-evenly px-16">
                <h2>{{ event.summary }}</h2>
                <p>{{ event.description }}</p>
                <p>{{ event.location }}</p>
                <p class="mt-auto">
                  {{
                    event.start.dateTime
                        ? format(new Date(event.start.dateTime), "dd/MM 'à' H'h'mm")
                        : event.start.date
                            ? format(new Date(event.start.date), "dd/MM")
                            : 'Date invalide'
                  }}
                </p></div>
            </CardContent>
          </Card>
        </router-link>

        <!--        <Card class="w-full col-span-3 p-4">-->
        <!--          <div class="flex gap-4">-->
        <!--            <h2 class="text-2xl">Historique</h2>-->
        <!--            <Select v-model="timePeriod">-->
        <!--              <SelectTrigger class="w-fit bg-white">-->
        <!--                <SelectValue class="w-fit">{{-->
        <!--                    timePeriod === 'all' ? 'Tout' : timePeriod === 'week' ? '7 derniers jours' : '30 derniers jours'-->
        <!--                  }}-->
        <!--                </SelectValue>-->
        <!--              </SelectTrigger>-->
        <!--              <SelectContent>-->
        <!--                <SelectGroup>-->
        <!--                  <SelectItem value="all">Tout</SelectItem>-->
        <!--                  <SelectItem value="week">7 derniers jours</SelectItem>-->
        <!--                  <SelectItem value="month">30 derniers jours</SelectItem>-->
        <!--                </SelectGroup>-->
        <!--              </SelectContent>-->
        <!--            </Select>-->
        <!--          </div>-->
        <!--          <LineChart-->
        <!--              :data="sortedData"-->
        <!--              index="date"-->
        <!--              xField="date"-->
        <!--              :categories="['Douleur', 'Activité physique']"-->
        <!--          />-->
        <!--        </Card>-->
      </div>
    </div>
  </section>

</template>

<script setup lang="ts">
import {computed, onMounted, ref} from 'vue'

import {type DateValue, getLocalTimeZone} from '@internationalized/date'

import {Card, CardContent, CardHeader, CardTitle,} from '@/components/ui/card'
import {Button} from '@/components/ui/button'
import apiService from "@/services/apiService";
import googleApiService from "@/services/googleApiService";
import {format} from 'date-fns';


const carnetSanteId = 1;
const donneesCarnetSante = ref();
const isLoading = ref(true);
const userId = 'd3cb0c2c-d405-4bc7-91a3-2022440d5267'

const upcomingEvents = ref<Event[]>([]);

onMounted(async () => {
  donneesCarnetSante.value = await apiService.getLastDonneesCarnetSante(carnetSanteId);
  console.log(donneesCarnetSante.value)
  isLoading.value = false;
  const urlParams = new URLSearchParams(window.location.search);
  let accessToken: any = urlParams.get('accessToken');
  let refreshToken: any = urlParams.get('refreshToken');

  // Store tokens in localStorage if they are in the URL
  if (accessToken) {
    localStorage.setItem('googleAccessToken', accessToken);
  }

  if (refreshToken) {
    localStorage.setItem('googleRefreshToken', refreshToken);
  }

  // If the tokens are not in the URL, try to get them from localStorage
  if (!accessToken) {
    accessToken = localStorage.getItem('googleAccessToken');
  }
  if (!refreshToken) {
    refreshToken = localStorage.getItem('googleRefreshToken');
  }

  // Redirect if both tokens are still null after attempting to retrieve them
  if (!accessToken && !refreshToken) {
    window.location.href = await googleApiService.getAuthorizationUrl(userId);
    return;
  }

  try {
    upcomingEvents.value = await googleApiService.getUpcomingEvents(accessToken);
    console.log(upcomingEvents.value)
    isLoading.value = false;
  } catch (error: any) {
    if ((error.status === 401 || error.response.data.error.message.includes('invalid authentication credentials')) && refreshToken) { // Invalid token
      try {
        // Use the refresh token to get a new access token
        accessToken = await googleApiService.refreshAccessToken(userId, refreshToken);
        localStorage.setItem('googleAccessToken', accessToken);

        // Retry the request with the new access token
        upcomingEvents.value = await googleApiService.getUpcomingEvents(accessToken);
        isLoading.value = false;
      } catch (refreshError) {
        console.error(refreshError);
        window.location.href = await googleApiService.getAuthorizationUrl(userId);
      }
    } else {
      console.error(error);
    }
  }
});

const value = ref<DateValue>()

interface Event {
  id: string;
  summary: string;
  description: string;
  location: string;
  start: {
    dateTime: string;
    date: string;
  };
}


// const data = computed(() => {
//   if (!donneesCarnetSante.value) {
//     return [];
//   }
//
//   const douleursData = donneesCarnetSante.value.donneesDouleur.$values.map((d: any) => {
//     const date = new Date(d.date);
//     const formattedDate = format(date, 'dd-MM-yyyy');
//     const hours = date.getHours().toString().padStart(2, '0');
//     const minutes = date.getMinutes().toString().padStart(2, '0');
//     const formattedTime = `${hours}h${minutes}`;
//     return {
//       date: `${formattedDate} ${formattedTime}`,
//       Douleur: d.intensite,
//     };
//   });
//
//   const activitesPhysiquesData = donneesCarnetSante.value.donneesActivitePhysique.$values.map((d: any) => {
//     const date = new Date(d.date);
//     const formattedDate = format(date, 'dd-MM-yyyy');
//     const hours = date.getHours().toString().padStart(2, '0');
//     const minutes = date.getMinutes().toString().padStart(2, '0');
//     const formattedTime = `${hours}h${minutes}`;
//     return {
//       date: `${formattedDate} ${formattedTime}`,
//       'Activité physique': d.intensite,
//     };
//   });
//
//   return [...douleursData, ...activitesPhysiquesData];
// });
//
// const sortedData = computed(() => {
//   const combinedData = [...data.value];
//   const tempData = {};
//
//   combinedData.forEach((item) => {
//     if (!tempData[item.date]) {
//       tempData[item.date] = {
//         date: item.date,
//         'Douleur': item.Douleur || 0,
//         'Activité physique': item['Activité physique'] || 0,
//       };
//     } else {
//       if (item.Douleur) tempData[item.date].Douleur = item.Douleur;
//       if (item['Activité physique']) tempData[item.date]['Activité physique'] = item['Activité physique'];
//     }
//   });
//
//   const result = Object.values(tempData);
//
//   return result.sort((a: any, b: any) => new Date(a.date.split(' ')[0].split('-').reverse().join('-') + 'T' + a.date.split(' ')[1].replace('h', ':')).getTime() - new Date(b.date.split(' ')[0].split('-').reverse().join('-') + 'T' + b.date.split(' ')[1].replace('h', ':')).getTime());
// });

const lastDouleurEntry = computed(() => {
  if (donneesCarnetSante.value && donneesCarnetSante.value.donneesDouleur) {
    const lastEntry = donneesCarnetSante.value.donneesDouleur;
    const date = new Date(lastEntry.date);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const time = `${hours}h${minutes}`;
    return {
      typeDouleur: lastEntry.typeDouleur,
      time: time,
      date: date.toLocaleDateString()
    };
  }
  return null;
});

const lastActiviteEntry = computed(() => {
  if (donneesCarnetSante.value && donneesCarnetSante.value.donneesActivitePhysique) {
    const lastEntry = donneesCarnetSante.value.donneesActivitePhysique;
    const date = new Date(lastEntry.date);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const time = `${hours}h${minutes}`;
    return {
      typeActivite: lastEntry.typeActivite,
      time: time,
      date: date.toLocaleDateString()
    };
  }
  return null;
});

const lastMedicamentEntry = computed(() => {
  if (donneesCarnetSante.value && donneesCarnetSante.value.donneesMedicament) {
    const lastEntry = donneesCarnetSante.value.donneesMedicament;
    const date = new Date(lastEntry.date);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const time = `${hours}h${minutes}`;
    console.log(lastEntry)
    return {
      nom: lastEntry.medicament.nom,
      heure: time,
      date: date.toLocaleDateString()
    };
  }
  return null;
});

// let timePeriod = ref('all');
//
// const filteredEntries = computed(() => {
//   if (!donneesCarnetSante.value) {
//     return [];
//   }
//
//   const now = new Date();
//   let startDate: Date;
//
//   if (timePeriod.value === 'week') {
//     startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 7);
//   } else if (timePeriod.value === 'month') {
//     startDate = new Date(now.getFullYear(), now.getMonth() - 1, now.getDate());
//   } else if (timePeriod.value === 'all') {
//     startDate = new Date(0);
//   }
//
//   const douleursData = donneesCarnetSante.value.donneesDouleur.$values
//       .filter((d: any) => new Date(d.date) >= startDate)
//       .sort((a: any, b: any) => new Date(a.date).getTime() - new Date(b.date).getTime())
//       .map((d: any) => {
//         const date = new Date(d.date);
//         const formattedDate = format(date, 'dd-MM-yyyy');
//         const hours = date.getHours().toString().padStart(2, '0');
//         const minutes = date.getMinutes().toString().padStart(2, '0');
//         const formattedTime = `${hours}h${minutes}`;
//         return {
//           ...d,
//           date: `${formattedDate} ${formattedTime}`,
//         };
//       });
//
//   const activitesPhysiquesData = donneesCarnetSante.value.donneesActivitePhysique.$values
//       .filter((d: any) => new Date(d.date) >= startDate)
//       .sort((a: any, b: any) => new Date(a.date).getTime() - new Date(b.date).getTime())
//       .map((d: any) => {
//         const date = new Date(d.date);
//         const formattedDate = format(date, 'dd-MM-yyyy');
//         const hours = date.getHours().toString().padStart(2, '0');
//         const minutes = date.getMinutes().toString().padStart(2, '0');
//         const formattedTime = `${hours}h${minutes}`;
//         return {
//           ...d,
//           date: `${formattedDate} ${formattedTime}`,
//         };
//       });
//
//   const combinedData = [...douleursData, ...activitesPhysiquesData];
//
//   return combinedData.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
// });

const formatDate = (date: Date | DateValue) => {
  const day = new Intl.DateTimeFormat('fr', {day: 'numeric'}).format(date instanceof Date ? date : date.toDate(getLocalTimeZone()));
  const monthYear = new Intl.DateTimeFormat('fr', {
    month: 'long',
    year: 'numeric'
  }).format(date instanceof Date ? date : date.toDate(getLocalTimeZone()));
  return `${day === '1' ? '1er' : day} ${monthYear}`;
};

</script>

<style scoped>

</style>