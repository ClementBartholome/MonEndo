<template>
  <div class="grid-container gap-12">
    <section class="flex flex-wrap w-full h-fit container mx-auto py-8 bg-clearer rounded-3xl shadow-md ml-auto">
      <div
          class="flex justify-between items-center w-full gap-4 mb-4 ">
        <Button variant="custom" @click="$emit('close')"
             class="flex gap-2 mr-44 items-center cursor-pointer hover:opacity-80 transition-opacity">
          <i class="material-symbols-outlined ">arrow_back</i>
          <span>Revenir en arrière</span>
        </Button>
        <div class="form-modal">
          <Dialog>
            <DialogTrigger class="flex gap-2 items-center cursor-pointer hover:opacity-80 transition-opacity">
              <Button variant="custom">
              <span>Ajouter une activité physique</span>
              <i class="material-symbols-outlined">add</i>
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle class="text-2xl">Ajouter une activité physique</DialogTitle>
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
                <div class="flex items-center gap-8">
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
                      <FormLabel>Intensité de l'activité</FormLabel>
                      <FormControl>
                        <Input type="number" placeholder="1 à 10" min="1" v-bind="componentField"/>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                </div>
                <FormField v-slot="{ componentField }" name="effetDouleur">
                  <FormItem>
                    <FormLabel>Effet sur la douleur (sur une échelle de 1 à 10)</FormLabel>
                    <FormControl>
                      <Input type="number" placeholder="1 à 10" v-bind="componentField"/>
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
      <div class="w-full">
        <div class="flex gap-4">
          <h2 class="text-2xl">Historique</h2>
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
        <div v-else-if="filteredEntries.length === 0" class="mt-8">Pas de données disponibles</div>
        <LineChart
            v-else
            :data="filteredEntries"
            :categories="['intensite']"
            index="date"
        />
      </div>
    </section>
    <section v-if="entries.length > 0"
             class="container !mt-0 mx-auto py-8 w-full bg-clearer rounded-3xl shadow-md ml-auto">
      <Datatable :entries="entries" :columns="columns">
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

const authStore = useAuthStore();

const columns = [
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
  intensite: z.number({
    required_error: 'L\'intensité de l\'activité est requise',
  }),
  effetDouleur: z.number({
    required_error: 'L\'effet sur la douleur est requis',
  }),
  commentaire: z.string().optional(),
}))

const form = useForm({
  validationSchema: formSchema,
})

const onSubmit = form.handleSubmit((values) => {
  const dateTimeString = `${values.date}T${values.time}`;
  const dateTime = new Date(dateTimeString);
  const timezoneOffset = dateTime.getTimezoneOffset() * 60 * 1000;
  const utcDateTime = new Date(dateTime.getTime() - timezoneOffset);

  const valuesWithCarnetSanteId = {
    ...values,
    date: utcDateTime,
    commentaire: values.commentaire ? values.commentaire : 'Pas de commentaire',
    carnetSanteId: authStore.user?.carnetSanteId,
  };
  apiService.postDonneesActivitePhysique(valuesWithCarnetSanteId);

  const valuesForView = {
    ...valuesWithCarnetSanteId, date: format(values.date, 'dd/MM/yyyy'),
    time: values.time.replace(":", "h")
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

</script>