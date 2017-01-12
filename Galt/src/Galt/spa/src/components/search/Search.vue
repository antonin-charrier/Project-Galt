<template>
    <div style="height: 100%">
        <input type="text" id="search-bar" class="search w3-light-grey" placeholder="Search on Galt" v-model="query"
        @focus="onFocus" @focusout="onFocusOut"/>
        <search-dropdown v-show="showDropDown" :packages="results" :working="working" :query="query"></search-dropdown>
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
                focused: false,
                losingFocus: false,
                results: [],
                firstRun: true
            }
        },
        computed: {
            showDropDown: function() {
                return this.focused && (this.working || !this.firstRun)
            }
        },
        components: {
            'search-dropdown': SearchDropdown
        },
        watch: {
            query: function() {
                var $this = this
                this.working = true
                this.firstRun = false
                this.search()
            }
        },
        methods: {
            search: _.debounce(function() {
                var response = postAsync("api/Search", "Search", AuthService.accessToken, {
                    searchTerm: this.query
                });

                response.then(function(r) {
                        this.results = r;
                        this.working = false;
                    }.bind(this),
                    function(r) {
                        this.working = false;
                    }.bind(this));
            }, 500),
            onFocus: function() {
                this.focused = true;
                this.losingFocus = false;
            },
            onFocusOut: function() {
                this.losingFocus = true;
                setTimeout(function() {
                    if (this.losingFocus) {
                        this.focused = false;
                        this.losingFocus = false;
                    }
                }.bind(this), 200);
            }
        }
    }
</script>
<style>
    .search {
        position: relative;
        left: 10px;
        width: 250px;
        box-sizing: border-box;
        border: 2px solid #ccc;
        border-radius: 4px;
        font-size: 16px;
        background-color: white;
        background-image: url('../../assets/searchicon.png');
        background-size: 25px;
        background-position: 10px 10px;
        background-repeat: no-repeat;
        padding: 12px 20px 12px 40px;
        -webkit-transition: width 0.4s ease-in-out;
        transition: width 0.4s ease-in-out;
    }
    
    .search:focus {
        width: 450px;
    }
</style>