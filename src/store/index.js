import { defineStore } from 'pinia'

export const current_state = defineStore('state', {
    state: () => ({
        value: 0,   //0:读者, 1:作者, 2:管理员
        roles : ['读者', '作者', '管理员']
    }),
    getters: {
        Role: (state) =>(num)=> state.roles[(state.value+num)%3]
    },
    actions: {
        reset(num){
            this.value = num
        },
        add(num) {
            this.value = (this.value + num) % 3
        }
    }
})