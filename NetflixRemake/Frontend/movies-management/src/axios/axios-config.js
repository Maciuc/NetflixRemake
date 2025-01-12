import axios from "axios";
import router from "../router";
import tokenService from "../services/token.service";
import store from "../store/vuex";

const axiosConfig = axios.create({
  baseURL: "/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
  },
});

axiosConfig.interceptors.request.use(
  async (config) => {
    // store.commit("TOGGLE_LOADER", true);
    const token = tokenService.getJwtToken();
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    // store.commit("TOGGLE_LOADER", false);
    Promise.reject(error);
  }
);

axiosConfig.interceptors.response.use(
  async (res) => {
    // store.commit("TOGGLE_LOADER", false);
    return res;
  },
  async (err) => {
    const originalConfig = err.config;
    if (
      originalConfig &&
      originalConfig.url != `/api/Auth/login` &&
      err.response
    ) {
      if (err.response.status == 401) {
        // const token=tokenService.getJwtToken()
        const tokens = {
          AccessToken: tokenService.getJwtToken(),
          RefreshToken: tokenService.getRefreshToken(),
        };
        return await axios
          .post(`/api/Auth/refresh-token`, tokens)
          .then((response) => {
            //--clear local storage
            // tokenService.setJwtToken(response.data.token);
            tokenService.setJwtToken(response.data.AccessToken);
            tokenService.setRefreshToken(response.data.RefreshToken);
            //we continue the original request with the new token
            originalConfig.headers.Authorization =
              "Bearer " + response.data.token;
            // store.commit("TOGGLE_LOADER", false);
            return axiosConfig(originalConfig);
          })
          .catch((error) => {
            console.log(error);
            //remove old tokens
            tokenService.removeAuthTokens();
            //if refreshToken is expired, user needs to login again
            router.push({
              name: "login",
            });
            // store.commit("TOGGLE_LOADER", false);
            return Promise.reject(_error);
          });
      }
    }
    // store.commit("TOGGLE_LOADER", false);
    return Promise.reject(err);
  }
);

export const $axios = axiosConfig;
