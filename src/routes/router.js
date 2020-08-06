import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

import Main from '../components/Main'
import Data from '../components/Data'

export default new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Main',
      component: Main,
    },   
    {
      path: '/data',
      name: 'Data',
      component: Data,
    },   
    {
      path: '*',
      redirect: '/'
    }
  ]
})