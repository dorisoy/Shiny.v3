﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoreLocation;


namespace Shiny.Locations
{
    public class GpsManagerDelegate : ShinyLocationDelegate
    {
        readonly Lazy<IEnumerable<IGpsDelegate>> delegates = ShinyHost.LazyResolve<IEnumerable<IGpsDelegate>>();
        readonly Subject<IGpsReading> readingSubject = new Subject<IGpsReading>();

        internal GpsRequest? Request { get; set; }


        public IObservable<IGpsReading> WhenGps() => this.readingSubject;
        public override async void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
        {
            var loc = locations.Last();
            var reading = new GpsReading(loc);
            await this.delegates.Value.RunDelegates(x => x.OnReading(reading));
            this.readingSubject.OnNext(reading);
        }


        //public override void Failed(CLLocationManager manager, NSError error)
        //{
        //    base.Failed(manager, error);
        //}
    }
}
