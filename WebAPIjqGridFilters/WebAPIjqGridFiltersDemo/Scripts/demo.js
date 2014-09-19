$(function () {
    $("#companies").jqGrid({
        url: 'api/Companies',
        datatype: "json",
        search: true,
        colNames: ['id', 'Name', 'Address'],
        colModel: [
            {
                name: 'Id',
                index: 'Id',
                hidden: true,
                key: true
            },
            {
                name: 'Name',
                index: 'Name',
                width: 200,
                searchoptions: {
                    sopt: ['cn']
                }
            },
            {
                name: 'Address',
                index: 'Address',
                width: 300,
                searchoptions: {
                    sopt: ['cn']
                }
            }
        ],
        rowNum: 2,
        rowList: [2, 4, 8],
        pager: '#companiesPager',
        sortname: 'id',
        viewrecords: true,
        sortorder: "desc",
        caption: "Companies",
    });


    jQuery("#companies")
        .jqGrid('navGrid', '#companiesPager')
        .jqGrid('filterToolbar', {
            stringResult: true,
            searchOnEnter: true,
            defaultSearch: 'ge'
        });
})