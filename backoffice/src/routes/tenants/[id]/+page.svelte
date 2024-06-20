<script lang="ts">
	import ChevronLeft from 'lucide-svelte/icons/chevron-left';

	import { Button } from '$lib/components/ui/button/index.js';
	import * as Card from '$lib/components/ui/card/index.js';
	import { Input } from '$lib/components/ui/input/index.js';
	import { Label } from '$lib/components/ui/label/index.js';
	import { toast } from 'svelte-sonner';
	import { enhance, applyAction } from '$app/forms';
	import { goto } from '$app/navigation';
	import * as AlertDialog from '$lib/components/ui/alert-dialog';

	let { data, form } = $props();

	const redirect = () => {
		goto('/tenants');
	};

	$effect(() => {
		if (data.errors) {
			const error = data.errors[0];

			setTimeout(() => {
				toast.error(error.message, {
					duration: 1000,
					onAutoClose: redirect,
					onDismiss: redirect
				});
			}, 100);
		}

		if (form?.errors) {
			toast.error(form.errors[0].message);
		}

		if (form?.success) {
			toast.success(form.success, {
				onAutoClose: redirect,
				onDismiss: redirect,
				duration: 1000
			});
		}
	});
</script>

{#if !data.errors}
	<main class="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-4">
		<div class="mx-auto grid max-w-[150rem] auto-rows-max gap-4 md:mx-48">
			<div class="flex items-center gap-4">
				<a href="/tenants">
					<Button variant="outline" size="icon" class="h-7 w-7">
						<ChevronLeft class="h-4 w-4" />
						<span class="sr-only">Back</span>
					</Button>
				</a>
				<h1
					class="flex-1 shrink-0 whitespace-nowrap text-xl font-semibold tracking-tight sm:grow-0"
				>
					Editing Tenant {data.id}
				</h1>
				<div class="hidden items-center gap-2 md:ml-auto md:flex">
					<a href="/tenants"><Button variant="outline" size="sm">Discard</Button></a>
				</div>
			</div>
			<div class="grid gap-4 md:grid-cols-[1fr_250px] lg:grid-cols-2 lg:gap-8">
				<div class="grid auto-rows-max items-start gap-4 lg:col-span-2 lg:gap-8">
					<form method="POST" action="?/update" use:enhance>
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
										<Input id="name" name="name" type="text" class="w-full" value={data.name} />
									</div>
								</div>
							</Card.Content>
							<Card.Footer>
								<Button size="sm" type="submit">Update Tenant</Button>
							</Card.Footer>
						</Card.Root>
					</form>
				</div>
				<div class="grid auto-rows-max items-start gap-4 lg:gap-8">
					<div>
						<AlertDialog.Root>
							<AlertDialog.Trigger
								><Button size="sm" variant="destructive">Delete</Button></AlertDialog.Trigger
							>
							<AlertDialog.Content>
								<AlertDialog.Header>
									<AlertDialog.Title>Are you absolutely sure?</AlertDialog.Title>
									<AlertDialog.Description>
										This action cannot be undone. This will permanently delete the tenant {data.name}.
									</AlertDialog.Description>
								</AlertDialog.Header>
								<AlertDialog.Footer>
									<form
										method="post"
										action="?/delete"
										use:enhance={({ action }) => {

											return async ({ result, action }) => {
												console.log(result, action)
												if (result.type === 'redirect') {
													applyAction(result)
												} 
											};
										}}
									>
										<AlertDialog.Cancel>Cancel</AlertDialog.Cancel>
										<AlertDialog.Action type="submit">Delete</AlertDialog.Action>
									</form>
								</AlertDialog.Footer>
							</AlertDialog.Content>
						</AlertDialog.Root>
					</div>
				</div>
			</div>
			<div class="flex items-center justify-center gap-2 md:hidden">
				<a href="/tenants"><Button variant="outline" size="sm">Discard</Button></a>
				<Button size="sm" type="submit">Update</Button>
				<Button size="sm" type="submit">Delete</Button>
			</div>
		</div>
	</main>
{/if}
