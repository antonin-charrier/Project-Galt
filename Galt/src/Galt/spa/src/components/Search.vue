<template>
    <input type="text" class="w3-input w3-light-grey" placeholder="Search on Galt" v-model="query"/>
</template>
<script>
    import _ from 'lodash'
    import {
        postAsync
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
                if (response && response.constructor === Array)
                    this.results = response;
                this.working = false;
            }, 500)
        }
    }
</script>