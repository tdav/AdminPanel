import Vue from "vue";
import Vuex from "vuex";
import Vars from "../config/globalVars";

Vue.use(Vuex);

export default {
  state: {
    metaEntities: [],
    headers: [],
    list: [],
  },

  mutations: {
    setMetaList(state, payload) {
      state.metaEntities = payload;
    },

    setEntityMeta(state, payload) {
      state.headers = payload;
    },

    setEntityData(state, payload) {
      state.list = payload;
    },
  },

  actions: {
    getMetaAction: async ({ commit }) => {
      try {
        const res = await fetch(Vars.ApiUrl + "admin");
        const data = await res.json();
        commit("setMetaList", data.entity);
      } catch (e) {
        console.log(e);
      }
    },

    getDataAction: async ({ commit }, meta) => {
      try {
        let headers = [];

       
        meta.fealdsList.forEach((el) => {
          headers.push({ text: el.name , value: el.name.toLowerCase() });
        });
        headers.push( { text: 'Actions', value: 'actions', sortable: false });

        commit("setEntityMeta", headers);

        const res = await fetch(Vars.ApiUrl + meta.route);
        const data = await res.json();

        commit("setEntityData", data);
      } catch (e) {
        console.log(e);
      }
    },
  },

  getters: {
    metaEntities(state) {
      return state.metaEntities;
    },

    entityMeta(state) {
      return state.headers;
    },

    entityData(state) {
      return state.list;
    },
  },
};
