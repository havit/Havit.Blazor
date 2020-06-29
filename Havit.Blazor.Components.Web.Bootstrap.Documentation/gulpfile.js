const gulp = require("gulp");
const sass = require("gulp-sass");
//compile scss into css
function style() {
	return gulp
		.src("wwwroot/src/scss/*.scss")
		.pipe(sass().on("error", sass.logError))
		.pipe(gulp.dest("wwwroot/css"));
}
function watch() {
	gulp.watch("wwwroot/src/scss/*.scss", style);

}
exports.style = style;
exports.watch = watch;