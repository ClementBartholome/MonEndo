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
              <span class="hide-xsm">Ajouter une prise</span>
              <i class="material-symbols-outlined">add</i>
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle class="text-2xl">Ajouter une prise</DialogTitle>
            </DialogHeader>
            <form class="flex flex-col gap-6" @submit.prevent="onSubmitPriseForm">
              <FormField v-slot="{ componentField }" name="nom">
                <FormItem>
                  <FormLabel>Nom du médicament</FormLabel>
                  <FormControl>
                    <Select v-model="prise.medicamentId">
                      <SelectTrigger>
                        <SelectValue/>
                      </SelectTrigger>
                      <SelectContent>
                        <SelectGroup v-if="medicaments.length > 0" label="Médicaments">
                          <SelectItem v-for="medicament in medicaments" :key="medicament.id" :value="medicament.id">
                            {{ medicament.nom }}
                          </SelectItem>
                        </SelectGroup>
                        <p v-else class="px-4 py-2">Pas de médicaments enregistrés</p>
                      </SelectContent>
                    </Select>
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              </FormField>
              <div class="flex items-center gap-8">
                <FormField v-slot="{ componentField }" name="date">
                  <FormItem>
                    <FormLabel>Date</FormLabel>
                    <FormControl>
                      <Input type="date" v-bind="componentField" v-model="prise.date"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
                <FormField v-slot="{ componentField }" name="time">
                  <FormItem>
                    <FormLabel>Heure</FormLabel>
                    <FormControl>
                      <Input type="time" v-bind="componentField" v-model="prise.time"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
              </div>
              <FormField v-slot="{ componentField }" name="commentaire">
                <FormItem>
                  <FormLabel>Un commentaire ? (optionnel)</FormLabel>
                  <FormControl>
                    <Input type="text" placeholder="Écrivez ici" v-bind="componentField" v-model="prise.commentaire"/>
                  </FormControl>
                  <FormMessage/>
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
    <section class="container !mt-0  mx-auto py-8 w-full bg-clearer rounded-3xl shadow-md ml-auto">
      <Datatable v-if="listePrises.length > 0" :entries="listePrises" :columns="columns"
                 :deleteFunction="deleteDonneesMedicament">
      </Datatable>
      <div v-else class="flex justify-center items-center h-32">
        <p class="text-2xl">Aucune prise enregistrée</p>
      </div>
    </section>
    <div class="flex-row-container w-full gap-8 mb-16">
      <section
          class="flex flex-col h-fit items-center gap-4 w-6/12 container py-8 bg-clearer rounded-3xl shadow-md ml-auto">
        <div class="flex gap-4">
          <h2 class="text-2xl flex gap-4">
            <i class="material-symbols-outlined text-3xl">pill</i>
            Traitements en cours
          </h2>
          <Dialog>
            <DialogTrigger class="flex items-center ml-auto gap-2">
              <Button variant="custom">
                <span class="material-symbols-outlined">add</span>
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle class="text-2xl">Ajouter un traitement</DialogTitle>
              </DialogHeader>
              <form class="flex flex-col gap-4" @submit.prevent="onSubmitTraitementForm">
                <FormField v-slot="{ componentField }" name="nom">
                  <FormItem>
                    <FormLabel>Nom du médicament</FormLabel>
                    <FormControl>
                      <Input v-bind="componentField" v-model="traitement.nom"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
                <FormField v-slot="{ componentField }" name="posologie">
                  <FormItem>
                    <FormLabel>Posologie</FormLabel>
                    <FormControl>
                      <Input v-bind="componentField" v-model="traitement.posologie"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
                <FormField v-slot="{ componentField }" name="dateDebutTraitement">
                  <FormItem>
                    <FormLabel>Date de début du traitement</FormLabel>
                    <FormControl>
                      <Input type="date" v-bind="componentField" v-model="traitement.dateDebutTraitement"/>
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                </FormField>
                <Button class="mt-4" variant="custom" type="submit">
                  Enregistrer
                </Button>
              </form>
            </DialogContent>
          </Dialog>
        </div>
        <ul class="w-full">
          <li v-for="traitement in traitementsEnCours" :key="traitement.id" class="relative">
            <Dialog>
              <DialogTrigger @click="editTraitement(traitement.id)" class="absolute top-0 right-0 cursor-pointer">
                <span class="material-symbols-outlined edit-btn">edit</span>
              </DialogTrigger>
              <DialogContent>
                <DialogHeader>
                  <DialogTitle class="text-2xl">Modifier le traitement</DialogTitle>
                </DialogHeader>
                <form @submit.prevent="onSubmitEditTraitement" class="flex flex-col gap-4">
                  <FormField v-slot="{ componentField }" name="nom">
                    <FormItem>
                      <FormLabel>Nom du médicament</FormLabel>
                      <FormControl>
                        <Input v-bind="componentField" v-model="traitementForm.nom"/>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                  <FormField v-slot="{ componentField }" name="posologie">
                    <FormItem>
                      <FormLabel>Posologie</FormLabel>
                      <FormControl>
                        <Input v-bind="componentField" v-model="traitementForm.posologie"/>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                  <FormField v-slot="{ componentField }" name="dateDebutTraitement">
                    <FormItem>
                      <FormLabel>Date de début du traitement</FormLabel>
                      <FormControl>
                        <Input type="date" v-bind="componentField" v-model="traitementForm.dateDebutTraitement"/>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                  <FormField v-slot="{ componentField }" name="traitementEnCours">
                    <FormItem>
                      <FormControl>
                        <FormLabel>Traitement en cours </FormLabel>
                        <div class="flex items-center">
                          <input type="radio" v-bind="componentField" v-model="traitementForm.traitementEnCours"
                                 value="true"/>
                          <label class="mx-2">Oui</label>
                          <input type="radio" v-bind="componentField" v-model="traitementForm.traitementEnCours"
                                 value="false"/>
                          <label class="ml-2">Non</label>
                        </div>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                  <FormField v-if="traitementForm.traitementEnCours === 'false'" v-slot="{ componentField }" name="dateFinTraitement">
                    <FormItem>
                      <FormControl>
                        <FormLabel>Date de fin du traitement</FormLabel>
                        <Input type="date" v-bind="componentField" v-model="traitementForm.dateFinTraitement"/>
                      </FormControl>
                      <FormMessage/>
                    </FormItem>
                  </FormField>
                  <Button class="mt-4" variant="custom" type="submit">
                    Enregistrer
                  </Button>
                </form>
              </DialogContent>
            </Dialog>
            <p class="text-headline font-bold text-xl">{{ traitement.nom }}</p>
            <p>{{ traitement.posologie }}</p>
            <p>Depuis le {{ format(traitement.dateDebutTraitement, 'dd-MM-yyyy').replace(/-/g, '/') }}</p>
          </li>
        </ul>
      </section>
      <section class="flex flex-col h-fit items-center gap-4 w-6/12 container py-8 bg-clearer rounded-3xl shadow-md ml-auto">
        <div class="flex gap-4">
          <h2 class="text-2xl self-start flex gap-4">
            <i class="material-symbols-outlined text-3xl">pill_off</i>
            Traitements passés
          </h2>
        </div>
        <ul class="w-full">
          <li v-for="traitement in traitementsPasses" :key="traitement.id" class="relative">
            <div>
              <p class="text-headline font-bold text-xl">{{ traitement.nom }}</p>
              <p>{{ traitement.posologie }}</p>
              <p>Traitement pris du {{ format(traitement.dateDebutTraitement, 'dd-MM-yyyy').replace(/-/g, '/') }} au {{ format(traitement.dateFinTraitement, 'dd-MM-yyyy').replace(/-/g, '/') }}</p>
            </div>
          </li>
        </ul>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import {onMounted, ref} from 'vue'


import {Button} from '@/components/ui/button'
import {FormControl, FormField, FormItem, FormLabel, FormMessage,} from '@/components/ui/form'
import {Input} from '@/components/ui/input'
import {Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue,} from '@/components/ui/select'

import Datatable from "@/components/Datatable.vue";

import apiService from "@/services/apiService";

import {useAuthStore} from '@/store/auth';
import {format, parseISO} from 'date-fns';
import {Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger,} from '@/components/ui/dialog'

const authStore = useAuthStore();
const traitementsEnCours = ref<Medicament[]>([]);
const medicaments = ref<Medicament[]>([]);
const traitementsPasses = ref<Medicament[]>([]);
const listePrises = ref<Entry[]>([]);
const prise = ref<PriseMedicament>({nom: '', date: '', time: '', commentaire: '', medicamentId: ''})
const traitement = ref({nom: '', posologie: '', dateDebutTraitement: ''});
const isLoading = ref(true);

type Entry = Record<string, any>;

const traitementForm: any = ref({
  id: '',
  nom: '',
  posologie: '',
  dateDebutTraitement: '',
  dateFinTraitement: '',
  traitementEnCours: '',
});

const columns: any = [
  {data: 'nom', title: 'Nom du médicament'},
  {data: 'date', title: 'Date'},
  {data: 'time', title: 'Heure'},
  {data: 'commentaire', title: 'Commentaire'},
  {
    data: null,
    defaultContent: '<span class="material-symbols-outlined delete-btn">delete</span>'
  },
];

const deleteDonneesMedicament = async (id: number) => {
  await apiService.deleteDonneesMedicament(id);
};

interface Medicament {
  id: string;
  nom: string;
  posologie: string;
  dateDebutTraitement: any;
  dateFinTraitement?: any;
  traitementEnCours: boolean;
}

interface PriseMedicament {
  nom: string;
  date: string;
  time: string;
  commentaire: string;
  medicamentId: string;
}

onMounted(() => {
  apiService.getDonneesCarnetSante(authStore.user!.carnetSanteId)
      .then(data => {
        medicaments.value = data.medicaments.$values.map(med => ({
          id: med.id,
          nom: med.nom,
          posologie: med.posologie
        }));

        listePrises.value = data.donneesMedicament.$values.map((d: Entry) => {
          const date = new Date(d.date);
          const hours = date.getHours().toString().padStart(2, '0');
          const minutes = date.getMinutes().toString().padStart(2, '0');
          const time = `${hours}h${minutes}`;
          return {
            id: d.id,
            nom: d.nomMedicament,
            date: format(date, 'dd-MM-yyyy').replace(/-/g, '/'),
            time: time,
            commentaire: d.commentaire ? d.commentaire : 'Pas de détails'
          };
        });

        traitementsEnCours.value = data.medicaments.$values.filter(med => med.traitementEnCours);

        traitementsPasses.value = data.medicaments.$values.filter(med => med.traitementEnCours == false);
        console.log(traitementsPasses.value)
        
        isLoading.value = false;
      });
});

const onSubmitPriseForm = () => {
  const values = prise.value;
  const {time, ...valuesForApi} = prise.value;
  const dateTimeString = `${valuesForApi.date}T${time}`;
  const dateTime = new Date(dateTimeString);
  const timezoneOffset = dateTime.getTimezoneOffset() * 60 * 1000;
  const utcDateTime = new Date(dateTime.getTime() - timezoneOffset);

  const valuesWithCarnetSanteId = {
    ...valuesForApi,
    date: utcDateTime,
    commentaire: valuesForApi.commentaire ? valuesForApi.commentaire : 'Pas de commentaire',
    carnetSanteId: authStore.user?.carnetSanteId,
  };

  apiService.postDonneesPriseMedicament(valuesWithCarnetSanteId);

  const medicamentName = medicaments.value.find(med => med.id === values.medicamentId)?.nom;

  const valuesForView = {
    ...values,
    nom: medicamentName,
    commentaire: valuesForApi.commentaire ? valuesForApi.commentaire : 'Pas de commentaire',
    date: format(values.date, 'dd/MM/yyyy'),
    time: values.time.replace(":", "h"),
  };
  listePrises.value.push(valuesForView);
};

const onSubmitTraitementForm = () => {
  const values = traitement.value;
  const valuesWithCarnetSanteId = {
    ...values,
    dateDebutTraitement: new Date(values.dateDebutTraitement),
    carnetSanteId: authStore.user?.carnetSanteId,
    traitementEnCours: true
  };

  apiService.postMedicament(valuesWithCarnetSanteId).then((response) => {
    const medicamentAdded: any = {
      ...valuesWithCarnetSanteId,
      id: response.id 
    };
    traitementsEnCours.value.push(medicamentAdded);
    medicaments.value.push(medicamentAdded); 
  });
};

function editTraitement(traitementId) {


  const traitementToEdit = traitementsEnCours.value.find(t => t.id === traitementId) || medicaments.value.find(m => m.id === traitementId);

  if (traitementToEdit) {
    // Ensure dateDebutTraitement is a string in ISO format
    let formattedDate;
    if (typeof traitementToEdit.dateDebutTraitement === 'string') {
      formattedDate = format(parseISO(traitementToEdit.dateDebutTraitement), 'yyyy-MM-dd');
    } else if (traitementToEdit.dateDebutTraitement instanceof Date) {
      formattedDate = format(traitementToEdit.dateDebutTraitement, 'yyyy-MM-dd');
    } else {
      console.error('dateDebutTraitement is not in a valid format:', traitementToEdit.dateDebutTraitement);
    }

    traitementForm.value = {...traitementToEdit, dateDebutTraitement: formattedDate};
  } else {
    console.log(`No treatment found with id: ${traitementId}`);
  }
}

const onSubmitEditTraitement = () => {
  const values = traitementForm.value;
  const valuesWithCarnetSanteId = {
    ...values,
    dateDebutTraitement: new Date(values.dateDebutTraitement),
    carnetSanteId: authStore.user?.carnetSanteId,
    traitementEnCours: values.traitementEnCours === 'true'
  };
  const traitementId: any = values.id;

  apiService.putDonneesMedicament(traitementId, valuesWithCarnetSanteId).then(() => {
    const indexEnCours = traitementsEnCours.value.findIndex(traitement => traitement.id === traitementId);
    const indexPasses = traitementsPasses.value.findIndex(traitement => traitement.id === traitementId);

    if (valuesWithCarnetSanteId.traitementEnCours) {
      if (indexEnCours !== -1) {
        traitementsEnCours.value[indexEnCours] = valuesWithCarnetSanteId;
      } else {
        traitementsEnCours.value.push(valuesWithCarnetSanteId);
      }
      if (indexPasses !== -1) {
        traitementsPasses.value.splice(indexPasses, 1);
      }
    } else {
      if (indexEnCours !== -1) {
        traitementsEnCours.value.splice(indexEnCours, 1);
      }
      if (indexPasses === -1) {
        traitementsPasses.value.push(valuesWithCarnetSanteId);
      }
    }
  });
};


</script>