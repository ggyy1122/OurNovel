//管理侧边导航栏的展开/折叠状态
import { defineStore } from 'pinia'

export const useSidebarStore = defineStore('sidebar', {
    //定义一个名为 sidebar 的 Store
  state: () => ({
    isCollapsed: false
  }),//定义 Store 的初始状态:侧边栏默认展开（isCollapsed 为 false）,定义了一个布尔值 isCollapsed，控制侧边栏是否折叠
  actions: {
    toggleCollapse() {
      this.isCollapsed = !this.isCollapsed
    }//toggleCollapse() 方法会切换 isCollapsed 的值,也即改变折叠侧边栏的展开状态
  }
})