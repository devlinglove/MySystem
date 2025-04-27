import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { AuthorizationGuard } from './shared/guards/authorization.guard';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    canActivate: [AuthorizationGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      //{ path: 'admin', loadChildren: () => import('./admin/admin.module').then(module => module.AdminModule) },
    ]
  },
  {
    path: 'account',
    component: LayoutComponent,
    loadChildren: () => import('./authentication/authentication.module').then((m) => m.AuthenticationModule),
    // children: [
    //   {
    //     path: '',
    //     loadChildren: () =>
    //       import('./authentication/authentication.module').then(
    //         (m) => m.AuthenticationModule
    //       ),
    //   },
    // ],
  },
  // {
  //   path: 'authentication',
  //   loadChildren: () => import('./authentication/authentication.module').then((a) => a.AuthenticationModule)
  // },
  { path: "**", component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
