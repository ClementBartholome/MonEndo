<template>
  <div class="flex-column-container">
    <div class="flex justify-between items-center w-full gap-4">
      <router-link to="/">
        <Button variant="custom"
                class="flex gap-2 items-center cursor-pointer hover:opacity-80 transition-opacity">
          <i class="material-symbols-outlined ">arrow_back</i>
          <span>Revenir en arrière</span>
        </Button>
      </router-link>
      <div class="form-modal">
        <Dialog>
          <DialogTrigger class="flex gap-2 items-center cursor-pointer hover:opacity-80 transition-opacity">
            <Button variant="custom">
              <span>Ajouter une session</span>
              <i class="material-symbols-outlined">add</i>
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle class="text-2xl">Ajouter une session</DialogTitle>
            </DialogHeader>
            <form class="mt-8 flex flex-col gap-6" @submit="onSubmit">
              <FormField v-slot="{ componentField }" name="typeActivite">
                <FormItem>
                  <FormLabel>Type d'activité</FormLabel>
                  <FormControl>
                    <Input type="text" placeholder="Course à pied" v-bind="componentField" :autofocus="false"/>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <div class="flex items-center gap-8">
                <FormField v-slot="{ componentField }" name="date">
                  <FormItem>
                    <FormLabel>Date</FormLabel>
                    <FormControl>
                      <Input type="date" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
                <FormField v-slot="{ componentField }" name="time">
                  <FormItem>
                    <FormLabel>Heure</FormLabel>
                    <FormControl>
                      <Input type="time" v-bind="componentField"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
              </div>
              <FormField v-slot="{ componentField }" name="duree">
                <FormItem>
                  <FormLabel>Durée de l'activité</FormLabel>
                  <FormControl>
                    <Input type="number" placeholder="Durée en minutes" v-bind="componentField"/>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <FormField v-slot="{ componentField }" name="intensite">
                <FormItem>
                  <FormLabel>Intensité</FormLabel>
                  <FormControl>
                    <Slider v-bind="componentField" :default-value="[5]" :max="10" :min="1" :step="1"/>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <FormField v-slot="{ componentField }" name="effetDouleur">
                <FormItem>
                  <FormLabel>Effet sur la douleur</FormLabel>
                  <FormControl>
                    <Slider v-bind="componentField" :default-value="[5]" :max="10" :min="1" :step="1"/>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <FormField v-slot="{ componentField }" name="commentaire">
                <FormItem>
                  <FormLabel>Un commentaire ? (optionnel)</FormLabel>
                  <FormControl>
                    <Input type="text" placeholder="Écrivez ici" v-bind="componentField"/>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <Button class="mt-4" variant="custom" type="submit" @click="onSubmit">
                Enregistrer
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>
    </div>
    <div class="flex-row-container w-full gap-8">
      <section
          class="flex flex-wrap h-full w-8/12 container py-8 px-4 bg-clearer rounded-3xl shadow-md ml-auto">
        <div class="w-full flex flex-col justify-center items-center">
          <div class="flex justify-center items-center gap-4">
            <h2 class="text-2xl self-start flex gap-4">
              <i class="material-symbols-outlined text-3xl">timeline</i>
              Historique
            </h2>
            <Select v-model="timePeriod">
              <SelectTrigger class="w-fit bg-white">
                <SelectValue class="w-fit">{{
                    timePeriod === 'all' ? 'Tout' : timePeriod === 'week' ? '7 derniers jours' : '30 derniers jours'
                  }}
                </SelectValue>
              </SelectTrigger>
              <SelectContent>
                <SelectGroup>
                  <SelectItem value="all">Tout</SelectItem>
                  <SelectItem value="week">7 derniers jours</SelectItem>
                  <SelectItem value="month">30 derniers jours</SelectItem>
                </SelectGroup>
              </SelectContent>
            </Select>
          </div>
          <div v-if="isLoading">Chargement des données...</div>
          <div v-else-if="filteredEntriesForChart.length === 0" class="mt-8">Pas de données disponibles</div>
          <LineChart
              v-else
              :data="filteredEntriesForChart"
              :categories="['intensite']"
              index="date"
          />
        </div>
      </section>
      <section
          class="flex flex-col h-auto items-center gap-4 w-4/12 container py-8 bg-clearer rounded-3xl shadow-md ml-auto">
        <div class="flex gap-4">
          <h2 class="text-2xl self-start flex gap-4">
            <i class="material-symbols-outlined text-3xl">trending_up</i>
            Tendances
          </h2>
          <Select v-model="averageIntensityPeriod"> <!-- Step 2 -->
            <SelectTrigger class="w-fit bg-white">
              <SelectValue class="w-fit">{{
                  averageIntensityPeriod === 'week' ? '7 derniers jours' : '30 derniers jours'
                }}
              </SelectValue>
            </SelectTrigger>
            <SelectContent>
              <SelectGroup>
                <SelectItem value="week">7 derniers jours</SelectItem>
                <SelectItem value="month">30 derniers jours</SelectItem>
              </SelectGroup>
            </SelectContent>
          </Select>
        </div>
        <p>Durée totale ({{ filteredEntriesForTendances.length }} {{ filteredEntriesForTendances.length > 1 ? 'entrées' : 'entrée' }})</p>
        <span class="text-5xl text-highlight">{{ totalSessionDuration }}</span>
        <!--        <i class="material-symbols-outlined text-7xl text-button">test</i>-->
      </section>
    </div>
    <section v-if="entries.length > 0"
             class="container !mt-0 mb-16 mx-auto py-8 w-full bg-clearer rounded-3xl shadow-md ml-auto">
      <Datatable :entries="entries" :columns="columns" :deleteFunction="deleteDonneesActivitePhysique">
        <thead>
        <tr>
          <th>Type d'activité</th>
          <th>Date</th>
          <th>Heure</th>
          <th>Durée</th>
          <th>Intensité</th>
          <th>Effet sur la douleur</th>
          <th></th>
        </tr>
        </thead>
      </Datatable>
    </section>
  </div>
</template>

<script setup lang="ts">
import {computed, onMounted, ref, watch} from 'vue'
import {useForm} from 'vee-validate'
import {toTypedSchema} from '@vee-validate/zod'
import * as z from 'zod'

import {Button} from '@/components/ui/button'
import {FormControl, FormField, FormItem, FormLabel, FormMessage,} from '@/components/ui/form'
import {Input} from '@/components/ui/input'
import {LineChart} from "@/components/ui/chart-line";
import {Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue,} from '@/components/ui/select'
import Datatable from "@/components/Datatable.vue";

import apiService from "@/services/apiService";

import {useAuthStore} from '@/store/auth';
import {format} from 'date-fns';
import {Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger,} from '@/components/ui/dialog'
import {parse} from 'date-fns';
import {Slider} from "@/components/ui/slider";

const authStore = useAuthStore();

const columns: any = [
  {data: 'typeActivite'},
  {data: 'date'},
  {data: 'time'},
  {data: 'duree'},
  {data: 'intensite'},
  {data: 'effetDouleur'},
  {
    data: null,
    defaultContent: '<span class="material-symbols-outlined delete-btn">delete</span>'
  },
];

const deleteDonneesActivitePhysique = async (id: number) => {
  await apiService.deleteDonneesActivitePhysique(id);
};

type Entry = Record<string, any>;
const entries = ref<Entry[]>([]);

const isLoading = ref(true);

onMounted(() => {
  apiService.getDonneesCarnetSante(authStore.user!.carnetSanteId)
      .then(data => {
        entries.value = data.donneesActivitePhysique.$values.map((d: Entry) => {
          const date = new Date(d.date);
          const hours = date.getHours().toString().padStart(2, '0');
          const minutes = date.getMinutes().toString().padStart(2, '0');
          const time = `${hours}h${minutes}`;
          return {
            id: d.id,
            typeActivite: d.typeActivite,
            date: format(date, 'dd-MM-yyyy').replace(/-/g, '/'),
            time: time,
            duree: d.duree + "min",
            intensite: d.intensite,
            effetDouleur: d.effetDouleur,
            commentaire: d.commentaire ? d.commentaire : 'Pas de détails'
          };
        });
        console.log(entries.value)
        isLoading.value = false;
      });
});


const formSchema = toTypedSchema(z.object({
  typeActivite: z.string({
    required_error: 'Le type d\'activité est requis',
  }),
  date: z.string({
    required_error: 'La date est requise',
  }),
  time: z.string({
    required_error: 'L\'heure est requise',
  }),
  duree: z.number({
    required_error: 'La durée de l\'activité est requise',
  }),
  intensite: z.array(z.number({
    required_error: 'L\'intensité est requise',
  })),
  effetDouleur: z.array(z.number({
    required_error: 'L\'effet sur la douleur est requis',
  })),
  commentaire: z.string().optional(),
}))

const form = useForm({
  validationSchema: formSchema,
  initialValues: {
    intensite: [5],
    effetDouleur: [5],
  }
});

const onSubmit = form.handleSubmit((values) => {
  const dateTimeString = `${values.date}T${values.time}`;
  const dateTime = new Date(dateTimeString);
  const timezoneOffset = dateTime.getTimezoneOffset() * 60 * 1000;
  const utcDateTime = new Date(dateTime.getTime() - timezoneOffset);

  const valuesWithCarnetSanteId = {
    ...values,
    intensite: values.intensite[0],
    effetDouleur: values.effetDouleur[0],
    date: utcDateTime,
    commentaire: values.commentaire ? values.commentaire : 'Pas de commentaire',
    carnetSanteId: authStore.user?.carnetSanteId,
  };
  apiService.postDonneesActivitePhysique(valuesWithCarnetSanteId);

  const valuesForView = {
    ...valuesWithCarnetSanteId, date: format(values.date, 'dd/MM/yyyy'),
    time: values.time.replace(":", "h"),
    intensite: values.intensite[0],
    effetDouleur: values.effetDouleur[0],
  };
  console.log(valuesForView)
  delete valuesForView.carnetSanteId;
  entries.value.push(valuesForView);
});

let timePeriod = ref('all');

const filteredEntries = computed(() => {
  const now = new Date();
  now.setHours(0, 0, 0, 0);
  let startDate;

  if (timePeriod.value === 'week') {
    startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 7);
  } else if (timePeriod.value === 'month') {
    startDate = new Date(now.getFullYear(), now.getMonth() - 1, now.getDate());
  } else if (timePeriod.value === 'all') {
    return entries.value.sort((a, b) => parse(a.date, 'dd/MM/yyyy', new Date()).getTime() - parse(b.date, 'dd/MM/yyyy', new Date()).getTime());
  } else {
    startDate = new Date(0);
  }
  startDate.setHours(0, 0, 0, 0);

  return entries.value.filter(entry => {
    const entryDate = parse(entry.date, 'dd/MM/yyyy', new Date());
    entryDate.setHours(0, 0, 0, 0);
    return entryDate.getTime() >= startDate.getTime();
  }).sort((a, b) => parse(a.date, 'dd/MM/yyyy', new Date()).getTime() - parse(b.date, 'dd/MM/yyyy', new Date()).getTime());
});

const filteredEntriesForChart = computed(() => {
  return filteredEntries.value.map(entry => {
    const {id, ...entryWithoutId} = entry;
    return entryWithoutId;
  });
});

let averageIntensityPeriod = ref('week');

const filteredEntriesForTendances = computed(() => {
  const now = new Date();
  now.setHours(0, 0, 0, 0);
  let startDate;

  if (averageIntensityPeriod.value === 'week') {
    startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 7);
  } else if (averageIntensityPeriod.value === 'month') {
    startDate = new Date(now.getFullYear(), now.getMonth() - 1, now.getDate());
  } else {
    startDate = new Date(0);
  }
  startDate.setHours(0, 0, 0, 0);

  return entries.value.filter(entry => {
    const entryDate = parse(entry.date, 'dd/MM/yyyy', new Date());
    entryDate.setHours(0, 0, 0, 0);
    return entryDate.getTime() >= startDate.getTime();
  });
});

const totalSessionDuration = computed(() => {
  const totalMinutes = filteredEntriesForTendances.value.reduce((total, entry) => total + Number(entry.duree.replace('min', '')), 0);
  const hours = Math.floor(totalMinutes / 60);
  const minutes = totalMinutes % 60;
  return `${hours}h${minutes.toString().padStart(2, '0')}`;
});

// const averageIntensity = computed(() => {
//   const now = new Date();
//   now.setHours(0, 0, 0, 0);
//   let startDate;
//
//   if (averageIntensityPeriod.value === 'week') { // Step 3
//     startDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 7);
//   } else if (averageIntensityPeriod.value === 'month') { // Step 3
//     startDate = new Date(now.getFullYear(), now.getMonth() - 1, now.getDate());
//   } else {
//     startDate = new Date(0);
//   }
//   startDate.setHours(0, 0, 0, 0);
//
//   const lastWeekEntries = entries.value.filter(entry => {
//     const entryDate = parse(entry.date, 'dd/MM/yyyy', new Date());
//     entryDate.setHours(0, 0, 0, 0);
//     return entryDate.getTime() >= startDate.getTime();
//   });
//
//   if (lastWeekEntries.length === 0) {
//     return 'N/A';
//   }
//
//   const totalIntensity = lastWeekEntries.reduce((total, entry) => total + Number(entry.intensite), 0);
//   return (totalIntensity / lastWeekEntries.length).toFixed(2);
// });
//
// const intensityIcon = computed(() => {
//   const avgIntensity = Number(averageIntensity.value);
//   if (avgIntensity >= 0 && avgIntensity < 2) {
//     return 'sentiment_very_satisfied';
//   } else if (avgIntensity >= 2 && avgIntensity < 4) {
//     return 'sentiment_satisfied';
//   } else if (avgIntensity >= 4 && avgIntensity < 6) {
//     return 'sentiment_neutral';
//   } else if (avgIntensity >= 6 && avgIntensity < 8) {
//     return 'sentiment_dissatisfied';
//   } else if (avgIntensity >= 8 && avgIntensity <= 10) {
//     return 'sentiment_very_dissatisfied';
//   } else {
//     return '';
//   }
// });
</script>