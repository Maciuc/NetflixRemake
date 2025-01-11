import { createRouter, createWebHistory } from "vue-router";
import MovieList from "@/components/MovieList.vue";
import MovieForm from "@/components/MovieForm.vue";
import MovieEdit from "@/components/MovieEdit.vue";

const routes = [
  { path: "/", component: MovieList },
  { path: "/add", component: MovieForm },
  { path: "/edit/:id", component: MovieEdit, props: true }, // Rută pentru editare
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
