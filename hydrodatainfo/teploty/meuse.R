library(gstat)
data(meuse)
names(meuse)
hist(meuse$zinc)
plot(meuse$x, meuse$y, asp=1)

coordinates(meuse) = ~x + y
class(meuse)

data(meuse.grid)
str(meuse.grid)
coordinates(meuse.grid) = ~x + y
gridded(meuse.grid) <- TRUE
plot(meuse.grid$dist)

zn.tr1 <- krige(log(zinc) ~ x + y, meuse, meuse.grid)
image(zn.tr1)

zn.lm <- lm(log(zinc) ~ x + y + I(x*y) + I(y^2), meuse)
vgm1 <- variogram(log(zinc)~1, meuse)
plot(vgm1, plot.numbers=TRUE, pch="+")

#universal kriging!
zn.lm <- lm(log(zinc) ~ sqrt(dist), meuse)
meuse$fitted.s <- predict(zn.lm, meuse)-mean(predict(zn.lm, meuse))
meuse$residuals <- residuals(zn.lm)

plot(zn.lm)

hist(meuse$residuals)
spplot(meuse, c("fitted.s", "residuals"))
vm.uk <- variogram(log(zinc)~sqrt(dist), meuse)
plot(vm.uk, plot.numbers=TRUE, pch="+")
m.uk <- fit.variogram(vm.uk, vgm(.3, "Sph", 800, 0.06))
ko.uk<- krige(log(zinc)~x+y, meuse, meuse.grid, model=m.uk)