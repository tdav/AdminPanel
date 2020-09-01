<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      :clipped="$vuetify.breakpoint.lgAndUp"
      app
    >
      <v-list dense>
        <template v-for="item in metaEntities">
          <v-list-item @click="selectItem(item)" :key="item.name" link>
            <v-list-item-action>
              <!-- <v-icon>{{ item.iconUrl }}</v-icon> -->
              <v-icon>mdi-contacts </v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>
                {{ item.name }}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      :clipped-left="$vuetify.breakpoint.lgAndUp"
      app
      color="blue darken-3"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title style="width: 300px" class="ml-0 pl-4">
        <span class="hidden-sm-and-down">Admin Panel</span>
      </v-toolbar-title>
      <v-text-field
        flat
        solo-inverted
        hide-details
        prepend-inner-icon="mdi-magnify"
        label="Search"
        class="hidden-sm-and-down"
      ></v-text-field>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-main>
      <v-container fluid>
        <v-row>
          <v-col col="12">
            <router-view></router-view>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  props: {
    source: String,
  },
  data: () => ({
    dialog: false,
    drawer: null,
  }),


  computed: mapGetters(["metaEntities"]),
  methods: {
    ...mapActions(["getMetaAction", "getDataAction"]),

    selectItem(it) {
      this.getDataAction(it);
    },
  },

  async mounted() {
    this.getMetaAction();
  },
};
</script>

<style>
html {
  overflow-y: auto;
}
</style>
