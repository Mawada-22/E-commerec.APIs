import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

// One reusable component for the static "company" pages (About / Privacy
// Policy / Careers) - the page key comes from the route's data.
@Component({
  selector: 'app-info-page',
  templateUrl: './info-page.component.html',
  styleUrls: ['./info-page.component.scss']
})
export class InfoPageComponent implements OnInit {
  page: string;

  content = {
    about: {
      title: 'About Us',
      icon: 'fa-heart',
      paragraphs: [
        'Mawada\'s Kitchen started with one simple idea: food tastes better when it\'s made with love.',
        'Every dish on our menu is prepared fresh, using recipes collected and perfected over the years — from rich Italian classics to comforting soups and homestyle favourites.',
        'We deliver across town so you can enjoy a homemade meal without leaving your couch. Thank you for being part of our story! 💜'
      ]
    },
    policy: {
      title: 'Privacy Policy',
      icon: 'fa-shield',
      paragraphs: [
        'We only collect the information we need to prepare and deliver your order: your name, delivery address, and contact details.',
        'Your payment details never touch our servers — all card payments are processed securely by Stripe.',
        'We never sell or share your personal data with third parties. You can ask us to delete your account and data at any time by contacting hello@mawadaskitchen.com.'
      ]
    },
    careers: {
      title: 'Careers',
      icon: 'fa-star',
      paragraphs: [
        'Love food as much as we do? We\'re always happy to meet passionate cooks, friendly delivery riders, and creative marketers.',
        'We offer flexible hours, a supportive kitchen family, and free lunch (of course).',
        'Send your CV to hello@mawadaskitchen.com with the role you\'re interested in — we read every single one.'
      ]
    }
  };

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => this.page = data.page);
  }

  get current() {
    return this.content[this.page] || this.content.about;
  }
}
