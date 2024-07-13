<script setup lang="ts">
import type { HTMLAttributes } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps<{
  class?: HTMLAttributes['class']
}>()
</script>

<template>
  <article
      style="max-width: 280px;"
    :class="
      cn(
        'rounded-2xl border-0 bg-card text-card-foreground shadow-sm h-full cursor-pointer transition-shadow duration-300 cursor-pointer hover:shadow-lg hover:shadow-gray-400',
        props.class,
      )
    "
  >
    <slot />
  </article>
</template>
