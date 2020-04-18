<template>
  <div>
    <div v-for="brick in bricks" v-bind:key="brick.id">
      <label>Время отреза:</label>
      &#8195;{{brick.dt}}
    </div>
  </div>
</template>

<script>
export default {
  props: ["bricks"]
};
</script>