import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const toastr = inject(ToastrService);



  return next(req).pipe(
    catchError(errorResponse => {
      if (errorResponse) {

        switch (errorResponse.status) {
          case 400:
            if (errorResponse.error.errors) {
              const modalStateErrors = [];
              for (const key in errorResponse.error.errors) {
                if (errorResponse.error.errors[key]) {
                  modalStateErrors.push(errorResponse.error.errors[key]);
                }
              }
              throw modalStateErrors.flat();
            } else {
              toastr.error(errorResponse.error, errorResponse.status);
            }
            break;

          case 401:
            toastr.error("Unauthorized", errorResponse.status);

            break;
          case 404:
            router.navigateByUrl("/not-found");
            break;

          case 500:
            const navigationExtras: NavigationExtras = {
              state: {
                error: errorResponse.error
              }
            }
            router.navigateByUrl("/server-error", navigationExtras);
            break;

          default:
            toastr.error("Something unexpected went wrong");
            break;
        }
      }
      throw errorResponse;
    })
  );
};
