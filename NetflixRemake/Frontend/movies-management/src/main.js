import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import utils from "./utils/utils";
import { $axios } from "./axios/axios-config";

const app = createApp(App);

app.config.globalProperties.$axios = {
  ...$axios,
};
app.config.globalProperties.$utils = utils;

app.use(router);
app.mount("#app");
