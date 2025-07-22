import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { createPinia } from 'pinia'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
import 'font-awesome/css/font-awesome.min.css';
import Vue3Toastify from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'
const pinia = createPinia()
pinia.use(piniaPluginPersistedstate)
const app = createApp(App)
app.use(router)
app.use(pinia)
app.use(Vue3Toastify, {
  autoClose: 3000,
  position: 'top-center',
  theme: 'dark',
  hideProgressBar: false,
  closeButton: true
})
app.mount("#app")



