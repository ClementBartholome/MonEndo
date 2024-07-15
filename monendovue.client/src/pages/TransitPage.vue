<template>
  <div class="flex-column-container">
    <div class="flex justify-between items-center w-full gap-4">
      <router-link to="/">
        <Button variant="custom"
                class="flex gap-2 items-center cursor-pointer hover:opacity-80 transition-opacity">
          <i class="material-symbols-outlined ">arrow_back</i>
          <span class="hide-xsm">Revenir en arrière</span>
        </Button>
      </router-link>
      <div class="form-modal">
        <Dialog>
          <DialogTrigger class="flex gap-2 items-center cursor-pointer hover:opacity-80 transition-opacity">
            <Button variant="custom">
              <span class="hide-xsm">Ajouter une entrée</span>
              <i class="material-symbols-outlined">add</i>
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle class="text-2xl">Ajouter une entrée</DialogTitle>
            </DialogHeader>
            <form class="flex flex-col gap-6" @submit.prevent="onSubmit">
              <FormField name="typeEvenement">
                <FormItem>
                  <FormLabel>Type de trouble</FormLabel>
                  <FormControl>
                    <Select v-model="entry.typeEvenement">
                      <SelectTrigger>
                        <SelectValue>
                          {{ entry.typeEvenement || 'Sélectionner un type de trouble' }}
                        </SelectValue>
                      </SelectTrigger>
                      <SelectContent>
                        <SelectGroup label="Type de trouble">
                          <SelectItem value="Diarrhée">Diarrhée</SelectItem>
                          <SelectItem value="Constipation">Constipation</SelectItem>
                          <SelectItem value="Crampes">Crampes</SelectItem>
                          <SelectItem value="Ballonnements">Ballonnements</SelectItem>
                          <SelectItem value="Autre">Autre</SelectItem>
                        </SelectGroup>
                      </SelectContent>
                    </Select>
                  </FormControl>
                </FormItem>
              </FormField>
              <FormField name="date">
                <FormItem>
                  <FormLabel>Date</FormLabel>
                  <FormControl>
                    <Input type="date" v-model="entry.date"/>
                  </FormControl>
                </FormItem>
              </FormField>
              <FormField name="time">
                <FormItem>
                  <FormLabel>Heure</FormLabel>
                  <FormControl>
                    <Input type="time" v-model="entry.time"/>
                  </FormControl>
                </FormItem>
              </FormField>
              <FormField name="intensite">
                <FormItem>
                  <FormLabel>Intensité</FormLabel>
                  <FormControl>
                    <Select v-model="entry.intensite">
                      <SelectTrigger>
                        <SelectValue>
                          {{ entry.intensite || 'Sélectionner un niveau d\'intensité' }}
                        </SelectValue>
                      </SelectTrigger>
                      <SelectContent>
                        <SelectGroup label="Intensité">
                          <SelectItem value="Légère">Légère</SelectItem>
                          <SelectItem value="Modérée">Modérée</SelectItem>
                          <SelectItem value="Sévère">Sévère</SelectItem>
                        </SelectGroup>
                      </SelectContent>
                    </Select>
                  </FormControl>
                </FormItem>
              </FormField>
              <FormField v-slot="{ componentField }" name="saignement">
                <FormItem>
                  <FormControl>
                    <FormLabel>Saignement</FormLabel>
                    <div class="flex items-center">
                      <input type="radio" v-bind="componentField" v-model="entry.saignement"
                             :value="true"/>
                      <label class="mx-2">Oui</label>
                      <input type="radio" v-bind="componentField" v-model="entry.saignement"
                             :value="false"/>
                      <label class="ml-2">Non</label>
                    </div>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <FormField v-slot="{ componentField }" name="douleurs">
                <FormItem>
                  <FormControl>
                    <FormLabel>Douleurs</FormLabel>
                    <div class="flex items-center">
                      <input type="radio" v-bind="componentField" v-model="entry.douleurs"
                             :value="true"/>
                      <label class="mx-2">Oui</label>
                      <input type="radio" v-bind="componentField" v-model="entry.douleurs"
                             :value="false"/>
                      <label class="ml-2">Non</label>
                    </div>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <FormField name="commentaire">
                <FormItem>
                  <FormLabel>Un commentaire ? (optionnel)</FormLabel>
                  <FormControl>
                    <Input type="text" placeholder="Écrivez ici" v-model="entry.commentaire"/>
                  </FormControl>
                </FormItem>
              </FormField>
              <Button class="mt-4" variant="custom" type="submit">
                Enregistrer
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>
    </div>
    <section v-if="entries.length > 0"
             class="container !mt-0 mb-16 mx-auto py-8 w-full bg-clearer rounded-3xl shadow-md ml-auto">
      <Datatable :entries="entries" :columns="columns" :deleteFunction="deleteDonneesTransit">
        <thead>
        <tr>
          <th>Type</th>
          <th>Date</th>
          <th>Heure</th>
          <th>Intensité</th>
          <th>Saignement</th>
          <th>Douleurs</th>
          <th></th>
        </tr>
        </thead>
      </Datatable>
    </section>
  </div>
</template>

<script setup lang="ts">

import {computed, onMounted, ref, watch} from 'vue'

import {Button} from '@/components/ui/button'
import {FormControl, FormField, FormItem, FormLabel, FormMessage,} from '@/components/ui/form'
import {Input} from '@/components/ui/input'
import {Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue,} from '@/components/ui/select'
import Datatable from "@/components/Datatable.vue";

import apiService from "@/services/apiService";

import {useAuthStore} from '@/store/auth';
import {format} from 'date-fns';
import {Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger,} from '@/components/ui/dialog'

const authStore = useAuthStore();

const columns: any = [
  {data: 'typeEvenement'},
  {data: 'date'},
  {data: 'time'},
  {data: 'intensite'},
  {data: 'saignement'},
  {data: 'douleurs'},
  {data: 'commentaire'},
  {
    data: null,
    defaultContent: '<span class="material-symbols-outlined delete-btn">delete</span>'
  },
];

const deleteDonneesTransit = async (id: number) => {
  await apiService.deleteDonneesTransit(id);
};

type Entry = Record<string, any>;
const entries = ref<Entry[]>([]);
const entry = ref<Entry>({
  typeEvenement: '',
  date: '',
  time: '',
  intensite: '', 
  saignement: false,
  douleurs: false,
  commentaire: ''
});

const isLoading = ref(true);

onMounted(async () => {
  const data = await apiService.getDonneesCarnetSante(authStore.user!.carnetSanteId);
  entries.value = data.donneesTransit.$values.map((d: Entry) => {
    const date = new Date(d.date);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const time = `${hours}h${minutes}`;
    return {
      ...d,
      date: format(date, 'dd-MM-yyyy').replace(/-/g, '/'),
      time,
      douleurs: d.douleur ? 'Oui' : 'Non',
      saignement: d.saignement ? 'Oui' : 'Non',
      commentaire: d.commentaires
    };
  });
  isLoading.value = false;
});


const onSubmit = () => {
  const values = entry.value;
  const dateTimeString = `${values.date}T${values.time}`;
  const dateTime = new Date(dateTimeString);
  const timezoneOffset = dateTime.getTimezoneOffset() * 60 * 1000;
  const utcDateTime = new Date(dateTime.getTime() - timezoneOffset);

  const valuesWithCarnetSanteId = {
    ...values,
    typeEvenement: entry.value.typeEvenement,
    date: utcDateTime,
    carnetSanteId: authStore.user?.carnetSanteId,
    douleur: entry.value.douleurs
  };
  apiService.postDonneesTransit(valuesWithCarnetSanteId);

  const valuesForView = {
    ...valuesWithCarnetSanteId,
    date: format(values.date, 'dd/MM/yyyy'),
    time: values.time.replace(":", "h"),
    douleurs: values.douleurs ? 'Oui' : 'Non',
    saignement: values.saignement ? 'Oui' : 'Non',
  };
  console.log(valuesForView);
  delete valuesForView.carnetSanteId;
  entries.value.push(valuesForView);
};


</script>

<style scoped>

</style>