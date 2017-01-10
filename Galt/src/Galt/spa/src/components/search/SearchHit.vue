<template>
    <div class="w3-hover-light-grey package-drop-div">
        <table class="package-drop-table">
            <tr>
                <td rowspan=2 valign="top" class="package-drop-icon-frame">
                    <img v-if="isIconUrlValid" class="package-drop-icon" :src="package.iconUrl"/>
                    <img v-else class="package-drop-icon" src="https://raw.githubusercontent.com/NuGet/Media/master/Images/MainLogo/64x64/nuget_64.png"/>
                </td>
                <td style="overflow: hidden">
                    <b>{{package.title}}</b>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="summary-box">
                        {{package.summary}}
                    </div>
                </td>
            </tr>
        </table>
    </div>
</template>
<script>
    import check from '../../helpers/urlChecker.js'

    export default {
        data: function() {
            return {
                imageHas404: false
            }
        },
        props: ['package'],
        computed: {
            isIconUrlValid: function() {
                return check(this.package.iconUrl) && !this.imageHas404;
            }
        }
    }
</script>
<style>
    .package-drop-icon {
        height: auto;
        width: 100%;
        max-width: 100%;
    }
    
    .package-drop-icon-frame {
        height: auto;
        width: 64px;
        max-width: 64px;
    }
    
    .package-drop-table {
        width: 100%;
        margin: 0 auto;
        max-width: 100%;
        height: 64px;
        max-height: 64px;
        overflow: hidden;
    }
    
    .package-drop-div {
        height: 64px;
        max-height: 64px;
        max-width: 512px;
        overflow: hidden;
    }
    
    .summary-box {
        width: 100%;
        text-overflow: ellipsis;
        overflow: hidden;
    }
</style>