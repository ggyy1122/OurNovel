import { defineStore } from 'pinia'

export const current_state = defineStore('state', {
    state: () => ({
        value: 0,   //0:读者, 1:作者, 2:管理员
        roles: ['读者', '作者', '管理员'],
        name: '',  //名称
        id: 0,   //ID
        isloggedin: false,  //是否已经登录
    }),
    getters: {
        Role: (state) => (num) => state.roles[(state.value + num) % 3]
    },
    actions: {
        reset(num) {
            this.value = num
        },
        add(num) {
            this.value = (this.value + num) % 3
        },
        setUserInfo(name, id) {   //登录，设置用户信息
            this.name = name;
            this.id = id;
            this.isloggedin = true;
        },
        clearUserInfo() {         //登出，清除用户信息
            this.name = '';
            this.id = '';
            this.isloggedin = false;
        }
    }
})