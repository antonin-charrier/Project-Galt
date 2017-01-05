<template>
    <input type="text" class="w3-input w3-light-grey" placeholder="Search on Galt" v-model="query"/>
</template>
<script>
    import _ from 'lodash'
    import {
        getAsyncWithData
    } from '../helpers/apiHelper.js'
    import AuthService from '../services/AuthService.js'

    export default {
        data: function() {
            return {
                query: "",
                working: false,
                results: []
            }
        },
        watch: {
            query: function() {
                this.working = true
                this.search()
            }
        },
        methods: {
            search: _.debounce(function() {
                var response = getAsyncWithData("api/Search", "Search", AuthService.accessToken, {
                    searchTerm: this.query
                });
                if (response && response.constructor === Array)
                    this.results = response;
                this.working = false;
            }.bind(this), 500)
        }
    }
</script>