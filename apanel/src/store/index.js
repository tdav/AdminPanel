import Vue from 'vue';
import Vuex from 'vuex';
import metaStore from './metaStore';
import metaItem from './metaItem';

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    metaStore,
    metaItem
  }
});
