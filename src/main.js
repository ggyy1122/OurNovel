import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { createPinia } from 'pinia'
import 'font-awesome/css/font-awesome.min.css';

const pinia = createPinia()
createApp(App).use(router).use(pinia).mount("#app");
