<script lang="ts">
	import ChevronLeft from 'lucide-svelte/icons/chevron-left';

	import { Button } from '$lib/components/ui/button/index.js';
	import * as Card from '$lib/components/ui/card/index.js';
	import { Input } from '$lib/components/ui/input/index.js';
	import { Label } from '$lib/components/ui/label/index.js';
	import { toast } from 'svelte-sonner';
	import { enhance } from '$app/forms';
	import { goto } from "$app/navigation"

	let { form }= $props();

	$effect(() => {
		if (form?.success) {
			toast.success(form.success.message);
			goto(`/tenants/${form.success.data.id}`);
		}

		if (form?.errors) {
			toast.error(`${form.errors[0].message}`);
		}
	});
</script>

<main class="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-4">
	<form method="POST" use:enhance>
		<div class="mx-auto grid max-w-[150rem] auto-rows-max gap-4 md:mx-48">
			<div class="flex items-center gap-4">
					<Button variant="outline" size="icon" class="h-7 w-7" onclick={(() => history.back())}>
						<ChevronLeft class="h-4 w-4" />
						<span class="sr-only">Back</span>
					</Button>
				<h1
					class="flex-1 shrink-0 whitespace-nowrap text-xl font-semibold tracking-tight sm:grow-0"
				>
					New Tenant
				</h1>
				<div class="hidden items-center gap-2 md:ml-auto md:flex">
					<a href="/tenants"><Button variant="outline" size="sm">Discard</Button></a>
					<Button size="sm" type="submit">Save Tenant</Button>
				</div>
			</div>
			<div class="grid auto-rows-max items-start gap-4 lg:col-span-full lg:gap-8">
				<Card.Root>
					<Card.Header>
						<Card.Title>Tenant Details</Card.Title>
						<Card.Description>
							This information will be displayed on the tenant profile
						</Card.Description>
					</Card.Header>
					<Card.Content>
						<div class="grid gap-6">
							<div class="grid gap-3">
								<Label for="name">Name</Label>
								<Input id="name" name="name" type="text" class="w-full" />
							</div>
						</div>
					</Card.Content>
				</Card.Root>
			</div>
			<div class="flex items-center justify-center gap-2 md:hidden">
				<a href="/tenants"><Button variant="outline" size="sm">Discard</Button></a>
				<Button size="sm" type="submit">Create</Button>
			</div>
		</div>
	</form>
</main>
