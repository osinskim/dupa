import { animate, group, query, style, transition, trigger } from '@angular/animations';

// export const routeAnimations = trigger('routeAnimations', [
//   transition('* <=> *', [
//     style({ position: 'relative' }),
//     query(':enter, :leave', [
//       style({
//         position: 'absolute',
//         top: 0,
//         left: 0,
//         width: '100%'
//       })
//     ]),
//     query(':enter', [style({ left: '-10%', opacity: 0 })]),
//     group([
//       query(':leave', [animate('0.3s', style({ opacity: 0 }))], { optional: true }),
//       query(':enter', [animate('0.3s ease-out', style({ left: '0%', opacity: 1 }))])
//     ]),
//   ])
// ]);

export const slideInOut =
  trigger('slideInOut', [
    transition(':enter', [
      style({ height: 0 }), animate(200)
    ]),
    transition(':leave', [
      animate(200, style({ height: 0 }))
    ]),
  ]);