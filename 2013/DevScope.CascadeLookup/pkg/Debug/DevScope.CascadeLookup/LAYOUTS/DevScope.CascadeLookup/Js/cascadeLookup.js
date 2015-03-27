function loadDVCLDropdowns() {
    SP.SOD.executeFunc("SP.js", "SP.ClientContext", function () {
        var clientContext = new SP.ClientContext.get_current();
        var site = clientContext.get_site();
        clientContext.load(site);
        clientContext.executeQueryAsync(Function.createDelegate(this, function () {
            serverUrl = site.get_url();

            // Poll for jQuery to come into existance
            var checkJQueryReady = function (callback) {
                if (window.jQuery) {
                    callback();
                }
                else {
                    window.setTimeout(function () { checkJQueryReady(callback); }, 20);
                }
            };

            // Poll for jQuery to come into existance
            var checkJQueryUIReady = function (callback) {
                if (window.jQuery.ui) {
                    callback();
                }
                else {
                    window.setTimeout(function () { checkJQueryUIReady(callback); }, 20);
                }
            };

            // Poll for knockout to come into existance
            var checkKnockoutReady = function (callback) {
                if (window.ko) {
                    callback();
                }
                else {
                    window.setTimeout(function () { checkKnockoutReady(callback); }, 20);
                }
            };

            // Poll for cascade dropdown to come into existance
            var checkCascadeDropdownReady = function (callback) {
                if (window.jQuery().cascadingDropdown) {
                    callback();
                }
                else {
                    window.setTimeout(function () { checkCascadeDropdownReady(callback); }, 20);
                }
            };

            // check for jquery
            if (!window.jQuery) {
                var script = document.createElement('script');
                script.type = "text/javascript";
                script.src = "/_layouts/15/DevScope.CascadeLookup/Js/jquery-1.11.2.min.js";
                document.getElementsByTagName('head')[0].appendChild(script);
            }

            // Start polling...
            checkJQueryReady(function () {
                // check for jquery ui
                if (!window.jQuery.ui) {
                    var script = document.createElement('script');
                    script.type = "text/javascript";
                    script.src = "/_layouts/15/DevScope.CascadeLookup/Js/jquery-ui-1.10.4.min.js";
                    document.getElementsByTagName('head')[0].appendChild(script);
                }

                checkJQueryUIReady(function () {
                    // check for knockout
                    if (!window.ko) {
                        var script = document.createElement('script');
                        script.type = "text/javascript";
                        script.src = "/_layouts/15/DevScope.CascadeLookup/Js/knockout-3.1.0.min.js";
                        document.getElementsByTagName('head')[0].appendChild(script);
                    }

                    checkKnockoutReady(function () {
                        if (!window.jQuery().cascadingDropdown) {
                            // load cascade dropdown widget
                            var script = document.createElement('script');
                            script.type = "text/javascript";
                            script.src = "/_layouts/15/DevScope.CascadeLookup/Js/jquery.cascandingdropdown.js";
                            document.getElementsByTagName('head')[0].appendChild(script);

                            checkCascadeDropdownReady(function () {
                                var dropdownInfo = [];

                                $('.dvclCascadeDropdown').each(function () {
                                    var info = {};
                                    info.fieldID = $(this).data('fieldid');
                                    info.hasDependency = Boolean($(this).data('dependency'));
                                    info.listID = $(this).data('listguid');
                                    info.column = $(this).data('listcolumn');
                                    info.dependencyColumn = $(this).data('dependencycolumn');
                                    info.dependencyListColumn = $(this).data('dependencylistcolumn');
                                    info.required = Boolean($(this).data('required'));
                                    info.selectedValue = $(this).data('selectedvalue');
                                    var hidden = $('input[type="hidden"]', $(this));
                                    info.hiddenElement = hidden;
                                    dropdownInfo.push(info);

                                    var select = $('select', $(this));
                                    select.addClass('dvclDropdown' + info.fieldID);
                                });

                                // declare cascade dropdown script
                                dependencyArray = [];
                                var selectBoxes = [];
                                var selectedParentId = "";
                                $.each(dropdownInfo, function (i, val) {
                                    var selectBox = {}
                                    
                                    //PostBack Fix
                                    if (val.hiddenElement.val() != val.selectedValue) {
                                        val.selectedValue = val.hiddenElement.val();
                                    }

                                    selectBox.isLoadingClassName = 'cascading-dropdown-loading';
                                    selectBox.paramName = 'filterID';
                                    selectBox.selector = '.dvclDropdown' + val.fieldID;
                                    selectBox.source = serverUrl + "/_vti_bin/CascadeLookupService.svc/GetItems?listID=" + val.listID
                                        + "&columnName=" + val.column
                                        + "&filterColumn=" + val.dependencyColumn
                                        //+ "&filterID=" + selectedParentId.toString()
                                        + "&hasDependency=" + val.hasDependency
                                        + "&required=" + val.required
                                        + "&selectedItemId=" + val.selectedValue

                                    if (val.hasDependency) {
                                        selectBox.requires = []
                                        $.each(dependencyArray, function (i, val) {
                                            selectBox.requires.push(val);
                                        });
                                        selectBox.requireAll = true;
                                    }
                                    else
                                        // clear dependency
                                        dependencyArray = [];
                                    
                                    selectBox.onChange = function (event, value, requiredValues) {
                                        val.hiddenElement.val(value);
                                    };

                                    selectBoxes.push(selectBox);

                                    // add dropdown to
                                    dependencyArray.push(selectBox.selector);

                                    selectedParentId = val.selectedValue;
                                });

                                $('.dvclCascadeDropdown').first().closest('.ms-formtable').cascadingDropdown({
                                    textKey: 'label',
                                    valueKey: 'value',
                                    selectBoxes: selectBoxes
                                });
                            });
                        }
                    });
                });
            });
        }))
    });
}
