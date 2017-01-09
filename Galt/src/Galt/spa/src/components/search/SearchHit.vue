<template>
    <div style="height: 64px" class="w3-hover-light-grey">
        <table class="package-drop-table">
            <tr>
                <td rowspan=2 class="package-drop-icon-frame">
                    <img class="package-drop-icon" :src="package.iconUrl"/>
                </td>
                <td style="overflow: hidden">
                    <b>{{package.title}}</b>
                </td>
            </tr>
            <tr>
                <td  style="overflow: hidden">
                    {{package.summary}}
                </td>
            </tr>
        </table>
    </div>
</template>
<script>
    export default {
        props: ['package']
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
    }
</style>