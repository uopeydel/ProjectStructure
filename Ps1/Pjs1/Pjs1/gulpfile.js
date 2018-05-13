var gulp = require('gulp');
var del = require('del');

var paths = {
    scripts: ['scripts/**/*.js'],
};

gulp.task('clean', function () {
    return del(['wwwroot/scripts/**/*']);
});

gulp.task('default', function () {
    gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/scripts'))
});