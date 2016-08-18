/*
 jQuery UI Datepicker plugin wrapper
 @param [ui-date] {object} Options to pass to $.fn.datepicker() merged onto ui.config
*/

var mod = angular.module('ui.date', ['ui.config']);
mod.directive('uiDate', ['ui.config', function(uiConfig) {
    return {
      require: '?ngModel',
      link: function(scope, element, attrs, controller) {
        var getOptions = function() {
          return angular.extend({}, uiConfig.date, scope.$eval(attrs.uiDate));
        };
        var initDateWidget = function() {
          var opts = getOptions();

          // If we have a controller (i.e. ngModelController) then wire it up
          if (controller) {
            var updateModel = function() {
              scope.$apply(function() {
                controller.$setViewValue(element.datepicker("getDate"));
              });
            };
            if (opts.onSelect) {
              // Caller has specified onSelect, so call this as well as updating the model
              var userHandler = opts.onSelect;
              opts.onSelect = function(value, picker) {
                updateModel();
                return userHandler(value, picker);
              };
            } else {
              // No onSelect already specified so just update the model
              opts.onSelect = updateModel;
            }
            // In case the user changes the text directly in the input box
            element.bind('change', updateModel);

            // Update the date picker when the model changes
            controller.$render = function() {
              var date = controller.$viewValue;
              element.datepicker("setDate", date);
              // Update the model if we received a string
              if ( typeof date == 'string' ) {
                controller.$setViewValue(element.datepicker("getDate"));
              }
            };
          }
          // If we don't destroy the old one it doesn't update properly when the config changes
          element.datepicker('destroy');
          // Create the new datepicker widget
          element.datepicker(opts);
          // Force a render to override whatever is in the input text box
          controller.$render();
        };
        // Watch for changes to the directives options
        scope.$watch(getOptions, initDateWidget, true);
      }
    };
  }
]);


mod.filter('uidate', ['ui.config', function(uiConfig) {
  return function(date, format) {
    if ( format === null ) {
      format = uiConfig.date.dateFormat;
    }
    return $.datepicker.formatDate(format, date);
  };
}]);