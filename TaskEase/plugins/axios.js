// plugins/axios.js
import axios from 'axios';

export default defineNuxtPlugin((nuxtApp) => {
  const axiosInstance = axios.create({
    baseURL: 'http://localhost:5159/Usuario',
  });

  nuxtApp.provide('axios', axiosInstance);
});
