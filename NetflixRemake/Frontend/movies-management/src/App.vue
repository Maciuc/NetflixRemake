<template>
  <div id="app">
    <MovieForm @movieSaved="fetchMovies" />
    <MovieList ref="movieList" />
  </div>
</template>

<script>
import MovieList from "@/components/MovieList.vue";
import MovieForm from "@/components/MovieForm.vue";
import MovieEdit from "@/components/MovieEdit.vue";

export default {
  components: {
    MovieList,
    MovieForm,
    MovieEdit,
  },
  methods: {
    fetchMovies() {
      this.$refs.movieList.fetchMovies();
    },
  },
};
</script>
