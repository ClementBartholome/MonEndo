<template>
  <section class="container">
    <Button variant="custom" @click="refreshData" class="refresh-button mb-4">Actualiser les données</Button>
    <div v-if="loading">Chargement des données du calendrier...</div>
    <FullCalendar v-else :options="calendarOptions" />
    <div v-if="selectedDate && popperPosition" :style="{zIndex: 9999, position: 'absolute', top: `${popperPosition.y}px`, left: `${popperPosition.x}px`}">
      <div class="bg-white p-4 rounded-lg shadow-md">
        <i @click="selectedDate = null" class="material-symbols-outlined hover:opacity-70 transition-opacity cursor-pointer absolute" style="right: 10px; top: 10px;">close</i>
        <h2 class="text-lg">Ajouter un événement</h2>
        <form>
          <div class="flex flex-col gap-4">
            <Input type="text" placeholder="Titre" />
            <Input type="text" placeholder="Heure de début" />
            <Input type="text" placeholder="Heure de fin" />
            <Input type="text" placeholder="Lieu" />
            <Button type="submit" variant="custom">Ajouter</Button>
          </div>
        </form>
      </div>
    </div>
  </section>
</template>


<script setup lang="ts">
import {type Ref, ref, onMounted, onUnmounted, computed, watch} from 'vue'
import FullCalendar from '@fullcalendar/vue3'
import googleCalendarPlugin from '@fullcalendar/google-calendar';
import dayGridMonth from "@fullcalendar/daygrid";
import frLocale from '@fullcalendar/core/locales/fr';
import interactionPlugin from "@fullcalendar/interaction";
import 'vue-popperjs/dist/vue-popper.css';
import {Input} from '@/components/ui/input';
import {Button} from '@/components/ui/button';


const events = ref<any[]>([]);
const loading = ref(true);
const selectedDate = ref(null);
type PositionType = { x: number; y: number; };

const popperPosition: Ref<PositionType> = ref({ x: 0, y: 0 });

let refreshIntervalId;

const fetchEvents = async () => {
  console.log('fetching events')
  localStorage.removeItem('events')
  events.value = [];
  loading.value = true;
  const calendarOptions = {
    googleCalendarApiKey: 'AIzaSyAhhkbsrZFQoXhTA5C-TdkFgSqFZ-rNOYI',
    googleCalendarId: "clementoss@gmail.com"
  };

  let pageToken;
  do {
    const response = await fetch(`https://www.googleapis.com/calendar/v3/calendars/${calendarOptions.googleCalendarId}/events?key=${calendarOptions.googleCalendarApiKey}${pageToken ? '&pageToken=' + pageToken : ''}`)
    const data = await response.json()
    const filteredEvents = data.items.filter(item => item.summary && item.summary.includes('MED')) // filter events
    events.value = [...events.value, ...filteredEvents.map(item => ({ // map to FullCalendar event format
      title: item.summary,
      start: item.start.dateTime || item.start.date,
      end: item.end.dateTime || item.end.date,
      url: item.htmlLink
    }))]
    pageToken = data.nextPageToken;
  } while (pageToken)
  localStorage.setItem('events', JSON.stringify(events.value))
  loading.value = false;
};

onMounted(() => {
  const localEvents = localStorage.getItem('events')
  if (localEvents) {
    events.value = JSON.parse(localEvents)
    loading.value = false;
  } else {
    fetchEvents()
  }

  // Actualise les événements toutes les 60 secondes
  refreshIntervalId = setInterval(fetchEvents, 60000);
});

onUnmounted(() => {
  clearInterval(refreshIntervalId);
});

const refreshData = async () => {
  loading.value = true;
  await fetchEvents();
  loading.value = false;
};

let calendarOptions = computed(() => ({
  plugins: [googleCalendarPlugin, dayGridMonth, interactionPlugin],
  initialView: 'dayGridMonth',
  googleCalendarApiKey: 'AIzaSyAhhkbsrZFQoXhTA5C-TdkFgSqFZ-rNOYI',
  events: events.value,
  height: 850,
  locale: frLocale,
  dateClick: function(info) {
    selectedDate.value = info.dateStr;
    popperPosition.value = {
      x: info.jsEvent.clientX,
      y: info.jsEvent.clientY
    };
    console.log(popperPosition.value)
    console.log(info)
  },
}))

watch(events, () => {
  calendarOptions = computed(() => ({
    plugins: [googleCalendarPlugin, dayGridMonth, interactionPlugin],
    initialView: 'dayGridMonth',
    googleCalendarApiKey: 'AIzaSyAhhkbsrZFQoXhTA5C-TdkFgSqFZ-rNOYI',
    events: events.value,
    height: 850,
    locale: frLocale,
    dateClick: function(info) {
      selectedDate.value = info.dateStr;
      popperPosition.value = {
        x: info.jsEvent.clientX,
        y: info.jsEvent.clientY
      };
      console.log(popperPosition.value)
      console.log(info)
    },
  }))
})
</script>

<style scoped>

</style>