<template>
  <DataTable :data="entries" class="display custom-datatable " :columns="columns" :options="options">
    <slot></slot>
    <tr v-for="(entry) in entries" :key="entry.id" :data-id="entry.id">
      <td v-for="column in columns">{{ entry[column.data] }}</td>
      <td>
        <span class="material-symbols-outlined delete-btn" @click="deleteEntry(entry.id)">delete</span>
      </td>
    </tr>
  </DataTable>
</template>

<script setup lang="ts">
import {ref, watch, nextTick, onMounted} from 'vue'
import type {PropType} from 'vue'
import DataTable from 'datatables.net-vue3';
import DataTablesCore from 'datatables.net';
import type {Config} from 'datatables.net';


DataTable.use(DataTablesCore);

type Entry = Record<string, any>;
const entries = ref<Entry[]>([]);

type Column = { data: string };

const props = defineProps({
  entries: Array as () => Entry[],
  columns: Array as () => Column[],
  deleteFunction: Function as PropType<(id: number) => Promise<void>>,
});

onMounted(async () => {
  entries.value = props.entries!;
});

const options = {
  language: {
    url: '//cdn.datatables.net/plug-ins/2.0.8/i18n/fr-FR.json',
    search: "<span class=\"material-symbols-outlined\">\search\</span>",
    searchPlaceholder: 'Rechercher'
  },
  lengthChange: false,
  info: false,
  paging: false,
  order: [[1, 'desc']],
  createdRow: function (row: any, data: any) {
    (row as Element).setAttribute('data-id', data.id);
  },
} as Config;


const deleteEntry = async (id: number) => {
  const index = entries.value.findIndex(entry => entry.id === id);
  if (index !== -1) {
    entries.value.splice(index, 1);
    await props.deleteFunction!(id);
  } else {
    console.error(`Entry with id ${id} not found`);
  }
};

watch(entries, () => {
  nextTick(() => {
    const table = document.querySelector('.custom-datatable');
    if (table) {
      table.addEventListener('click', (event) => {
        const targetElement = event.target as Element;
        if (targetElement.matches('.delete-btn')) {
          const rowElement = targetElement.closest('tr');
          const id = Number(rowElement!.getAttribute('data-id'));
          deleteEntry(id);
        }
      });
    } else {
      console.error('Table element not found');
    }
  });
});
</script>

<style scoped>
@import 'datatables.net-dt';
</style>