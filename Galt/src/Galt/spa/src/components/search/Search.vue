<template>
    <div style="height: 100%">
        <input type="text" class="w3-input w3-light-grey" placeholder="Search on Galt" v-model="query"/>
        <search-dropdown :packages="results"></search-dropdown>
    </div>
</template>
<script>
    import _ from 'lodash'
    import {
        postAsync
    } from '../../helpers/apiHelper.js'
    import AuthService from '../../services/AuthService.js'
    import SearchDropdown from './SearchDropdown.vue'

    export default {
        data: function() {
            return {
                query: "",
                working: false,
                results: []
            }
        },
        components: {
            'search-dropdown': SearchDropdown
        },
        watch: {
            query: function() {
                var $this = this
                this.working = true
                this.search()
            }
        },
        methods: {
            search: _.debounce(function() {
                var response = postAsync("api/Search", "Search", AuthService.accessToken, {
                    searchTerm: this.query
                });

                response.then(function(r) {
                    this.results = r
                }.bind(this));

                this.working = false;
            }, 500)
        }
    }
</script>